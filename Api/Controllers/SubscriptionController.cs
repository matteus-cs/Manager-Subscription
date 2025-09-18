using Api.Database;
using Api.Dtos;
using Api.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/subscriptions")]
[ApiController]
public class SubscriptionController(ManagerSubscriptionDb managerSubscriptionDb) : ControllerBase
{
    private ManagerSubscriptionDb _db = managerSubscriptionDb;

    [HttpPost()]
    public async Task<ActionResult> Create([FromBody] CreateSubscriptionDto createSubscriptionDto)
    {
        var subscription = new Subscription
        {
            StartDate = createSubscriptionDto.StartDate,
            EndDate = createSubscriptionDto.EndDate,
            IsActive = createSubscriptionDto.IsActive,
            DueDate = createSubscriptionDto.DueDate,
            CustomerId = createSubscriptionDto.CustomerId,
            PlanId = createSubscriptionDto.PlanId
        };

        await _db.Subscriptions.AddAsync(subscription);
        await _db.SaveChangesAsync();

        return Created("", new { subscription.Id });
    }
}