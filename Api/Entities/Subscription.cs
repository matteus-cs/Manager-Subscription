namespace Api.Entities;

public class Subscription
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public bool IsActive { get; set; }

    public byte BillingDay { get; set; }

    public Customer Customer { get; set; } = default!;

    public Guid CustomerId { get; set; }

    public Plan Plan { get; set; } = default!;

    public Guid PlanId { get; set; }
}