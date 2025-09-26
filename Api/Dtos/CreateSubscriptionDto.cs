using System.ComponentModel.DataAnnotations;

namespace Api.Dtos;

public record CreateSubscriptionDto
(
    [Required]
    [DataType(DataType.Date)]
    DateOnly StartDate,

    [Required]
    [DataType(DataType.Date)]
    DateOnly EndDate,

    [Required]
    bool IsActive,

    [Required]
    [Range(1, 31)]
    byte BillingDay,

    [Required]
    Guid CustomerId,

    [Required]
    Guid PlanId
);