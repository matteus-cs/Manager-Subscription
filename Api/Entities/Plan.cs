namespace Api.Entities;

public class Plan
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Name { get; set; } = String.Empty;
    public string? Description { get; set; } = String.Empty;
    public decimal MonthlyPrice { get; set; }
}