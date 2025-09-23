using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Unicode;
using Api.Database;
using Api.Dtos;
using Api.Entities;
using Api.Gateways;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Api.Controllers;

[Route("api/customers")]
[ApiController]
public class CustomerController(ManagerSubscriptionDb managerSubscriptionDb, IPayment payment, IConfiguration config) : ControllerBase
{
    private ManagerSubscriptionDb _managerSubscriptionDb = managerSubscriptionDb;
    private IPayment _paymentGateway = payment;

    private IConfiguration _config = config;

    private readonly PasswordHasher<object> _passwordHasher = new();

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateCustomerDto createCustomerDto)
    {
        var customerAlreadyExists = await _managerSubscriptionDb.Customers.FirstOrDefaultAsync(c => c.Email == createCustomerDto.Email);
        if (customerAlreadyExists != null)
        {
            return BadRequest(new { Message = "Customer already exists" });
        }

        var customer = new Customer
        {
            Name = createCustomerDto.Name,
            Cpf = createCustomerDto.Cpf,
            Email = createCustomerDto.Email,
            Password = _passwordHasher.HashPassword(null!, createCustomerDto.Password),
            Address = new Address
            {
                Street = createCustomerDto.Address.Street,
                Neighborhood = createCustomerDto.Address.Neighborhood,
                City = createCustomerDto.Address.City,
                State = createCustomerDto.Address.State,
            },
            Phone = createCustomerDto.Phone
        };
        string customerGatewayId = await _paymentGateway.CreateCustomerAsync(customer);

        customer.PaymentGatewayId = customerGatewayId;

        await _managerSubscriptionDb.Customers.AddAsync(customer);
        await _managerSubscriptionDb.SaveChangesAsync();

        return Created($"/{customer.Id}", new { Id = customer.Id });
    }

    [HttpPost("login")]
    public async Task<ActionResult> LogIn([FromBody] LongInDto longInDto)
    {
        var responseToInvalidEmailOrPassword = new { message = "email or password invalid" };
        var (Email, Password) = longInDto;
        var customer = await _managerSubscriptionDb.Customers.FirstOrDefaultAsync(c => c.Email == Email);
        if (customer is null) return BadRequest(responseToInvalidEmailOrPassword);

        var isMatched = _passwordHasher.VerifyHashedPassword(null!, customer.Password, Password);
        if (isMatched == PasswordVerificationResult.Failed)
            return BadRequest(responseToInvalidEmailOrPassword);
        return Ok(new { token = GenerateToken(customer.Id) });
    }

    private string GenerateToken(Guid customerId)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, customerId.ToString()),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["jwtKey"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(double.Parse(_config["Jwt:ExpireMinutes"]!)),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}