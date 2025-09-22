namespace Api.Dtos;

public record SubscriptionDto
(
    DateOnly StartDate,

    DateOnly EndDate,

    bool IsActive,

    byte BillingDay,

    Guid? PlanId,

    List<InstallmentDto>? Installments
);