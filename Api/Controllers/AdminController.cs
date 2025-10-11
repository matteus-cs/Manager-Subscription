using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Api.Database;
using Api.Dtos;
using Api.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Api.Controllers;


[ApiController]
[Route("api/admins")]
public class AdminController(ManagerSubscriptionDb db, IConfiguration config) : ControllerBase
{
    private readonly ManagerSubscriptionDb _db = db;
    private readonly PasswordHasher<object> _passwordHasher = new();
    private readonly IConfiguration _config = config;


    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateAdminDto createAdminDto)
    {
        var (Name, Email, Password) = createAdminDto;
        if (await _db.Admins.FirstOrDefaultAsync(a => a.Email == Email) is not null) return UnprocessableEntity(new {message = "Admin already exists"});

        var admin = new Admin
        {
            Name = Name,
            Email = Email,
            Password = _passwordHasher.HashPassword(null!, Password),
        };
        _db.Admins.Add(admin);
        await _db.SaveChangesAsync();
        return Created("", new { id = admin.Id });
    }

    [HttpPost("login")]
    public async Task<ActionResult> login([FromBody] LongInDto longInDto)
    {
        var responseToInvalidEmailOrPassword = new { message = "email or password invalid" };
        var (Email, Password) = longInDto;
        var admin = await _db.Admins.FirstOrDefaultAsync(a => a.Email == Email);
        if (admin is null) return BadRequest(responseToInvalidEmailOrPassword);

        var isMatched = _passwordHasher.VerifyHashedPassword(null!, admin.Password, Password);
        if (isMatched == PasswordVerificationResult.Failed)
            return BadRequest(responseToInvalidEmailOrPassword);
        return Ok(new { token = GenerateToken(admin.Id) });
    }

    private string GenerateToken(Guid customerId)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, customerId.ToString()),
            new Claim(ClaimTypes.Role, "Admin")
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