using Api.Database;
using Api.Dtos;
using Api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers;

[Route("api/subscriptions")]
[ApiController]
public class SubscriptionController(ManagerSubscriptionDb managerSubscriptionDb) : ControllerBase
{
    private ManagerSubscriptionDb _db = managerSubscriptionDb;

    [HttpPost()]
    public async Task<ActionResult> Create([FromBody] CreateSubscriptionDto createSubscriptionDto)
    {
        if (!(await _db.Customers.AnyAsync(c => c.Id == createSubscriptionDto.CustomerId)))
            return NotFound(new {message = "Customer not found"});
        
        var plan = await _db.Plans.FindAsync(createSubscriptionDto.PlanId);
        if (plan is null)
        {
            return NotFound(new {message = "Plan not found"});
        }

        var subscription = new Subscription
        {
            StartDate = createSubscriptionDto.StartDate,
            EndDate = createSubscriptionDto.EndDate,
            IsActive = createSubscriptionDto.IsActive,
            BillingDay = createSubscriptionDto.BillingDay,
            CustomerId = createSubscriptionDto.CustomerId,
            PlanId = createSubscriptionDto.PlanId
        };

        await _db.Subscriptions.AddAsync(subscription);

        var durationMonths = (subscription.EndDate.Year - subscription.StartDate.Year) * 12
            + subscription.EndDate.Month - subscription.StartDate.Month;

        List<Installment> installments = [];
        for (var i = 1; i <= durationMonths; i++)
        {
            var dueDate = DateOnly
                .FromDateTime(DateTime.UtcNow)
                .AddMonths(i);
            installments.Add(
                new Installment
                {
                    DueDate = new DateOnly(dueDate.Year, dueDate.Month, subscription.BillingDay),
                    Amount = plan.MonthlyPrice,
                    SubscriptionId = subscription.Id
                }
            );
        }

        await _db.Installments.AddRangeAsync(installments);

        await _db.SaveChangesAsync();

        return Created("", new { subscription.Id });
    }
}