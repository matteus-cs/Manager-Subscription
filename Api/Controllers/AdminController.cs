using Api.Database;
using Api.Dtos;
using Api.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers;


[ApiController]
[Route("api/admins")]
public class AdminController(ManagerSubscriptionDb db) : ControllerBase
{
    private readonly ManagerSubscriptionDb _db = db;
    private readonly PasswordHasher<object> _passwordHasher = new();

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateAdminDto createAdminDto)
    {
        var (Name, Email, Password) = createAdminDto;
        if (await _db.Admins.FirstOrDefaultAsync(a => a.Email == Email) is Admin) return UnprocessableEntity(new {message = "Admin already exists"});
          
        var admin = new Admin
        {
            Name = Name,
            Email = Email,
            Password = _passwordHasher.HashPassword(null!, Password),
        };
        _db.Admins.Add(admin);
        await _db.SaveChangesAsync();
        return Created("", new {id = admin.Id});
    }
}