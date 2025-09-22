using Api.Entities;

public record InstallmentDto
(
    Guid Id,
    DateOnly DueDate,
    decimal Amount,
    InstallmentStatus Status, 
    Guid SubscriptionId
);