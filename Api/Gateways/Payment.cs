using Api.Entities;

namespace Api.Gateways;

public interface IPayment
{
    Task<string> CreateCustomerAsync(Customer customer);
}
