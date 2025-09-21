namespace Api.Entities;

public enum InstallmentStatus
{
    Paid,
    Pending,
    Overdue
}
public class Installment
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public DateOnly DueDate { get; set; }

    public decimal Amount { get; set; }

    public InstallmentStatus Status { get; set; } = InstallmentStatus.Pending;

    public Subscription? Subscription { get; set; }

    public Guid SubscriptionId { get; set; }
}