namespace Api.Entities;

public class Address
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public string Street { get; set; } = string.Empty;

    public string Neighborhood { get; set; } = string.Empty;

    public string City { get; set; } = string.Empty;

    public string State { get; set; } = string.Empty;
}