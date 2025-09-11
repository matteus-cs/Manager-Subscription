namespace Api.Entities;

public class Customer
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public string Name { get; set; } = String.Empty;

    public string Email { get; set; } = String.Empty;

    public string Cpf { get; set; } = String.Empty;

    public string Phone { get; set; } = String.Empty;

    public Address Address { get; set; } = default!;

    public string? PaymentGatewayId { get; set; } = String.Empty;
}