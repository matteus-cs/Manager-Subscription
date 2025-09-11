using Api.Database;
using Api.Dtos;
using Api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers;

[Route("api/customers")]
[ApiController]
public class CustomerController(ManagerSubscriptionDb managerSubscriptionDb) : ControllerBase
{
    private ManagerSubscriptionDb _managerSubscriptionDb = managerSubscriptionDb;

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateCustomerDto createCustomerDto)
    {
        var customerAlreadyExists = await _managerSubscriptionDb.Customers.FirstOrDefaultAsync(c => c.Email == createCustomerDto.Email);
        if (customerAlreadyExists != null)
        {
            return BadRequest(new {Message = "Customer already exists"});
        }
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        var customer = new Customer
        {
            Name = createCustomerDto.Name,
            Cpf = createCustomerDto.Cpf,
            Email = createCustomerDto.Email,
            Address = new Address
            {
                Street = createCustomerDto.Address.Street,
                Neighborhood = createCustomerDto.Address.Neighborhood,
                City = createCustomerDto.Address.City,
                State = createCustomerDto.Address.State,
            },
            Phone = createCustomerDto.Phone
        };

        await _managerSubscriptionDb.Customers.AddAsync(customer);
        await _managerSubscriptionDb.SaveChangesAsync();

        return Created("", new { Id = customer.Id });
    }
}