using Api.Database;
using Api.Dtos;
using Api.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/plans")]
[ApiController]
public class PlanController(ManagerSubscriptionDb managerSubscriptionDb) : ControllerBase
{
    private readonly ManagerSubscriptionDb _db = managerSubscriptionDb;


    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreatePlanDto createPlanDto)
    {

        var plan = new Plan
        {
            Name = createPlanDto.Name,
            Description = createPlanDto.Description,
            MonthlyPrice = createPlanDto.MonthlyPrice
        };

        await _db.Plans.AddAsync(plan);
        await _db.SaveChangesAsync();

        return Created("", new { plan.Id });
    }

}