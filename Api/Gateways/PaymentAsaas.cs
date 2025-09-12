using Api.Entities;
using Api.Gateways;
using System.Text.RegularExpressions;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text;

public class PaymentAsaas : IPayment
{
    private readonly HttpClient _httpClient;
    public PaymentAsaas(HttpClient httpClient, IConfiguration configuration)
    {
        var apiKeyAsaas = configuration["ApiKeyAsaas"] ?? throw new ArgumentException("Missing Api key Asaas");
        _httpClient = httpClient;
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "test");
        _httpClient.DefaultRequestHeaders.Add("access_token", apiKeyAsaas);
    }


    public async Task<string> CreateCustomerAsync(Customer customer)
    {
        var jsonBody = JsonSerializer.Serialize(new
        {
            name = customer.Name,
            cpfCnpj = ExtractCpfNumbers(customer.Cpf),
            mobilePhone = customer.Phone
        });

        var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

        HttpResponseMessage response;

        try
        {
            response = await _httpClient.PostAsync("https://api-sandbox.asaas.com/v3/customers", content);
        }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException("Error calling Asaas endpoint", ex);
        }

        var responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException(
                $"Error creating customer: {response.StatusCode}\nContent: {responseContent}");
        }

        try
        {
            var contentResponse = JsonSerializer.Deserialize<CreateCustomerResponse>(responseContent)
                ?? throw new InvalidOperationException("Failed to deserialize Asaas response");

            return contentResponse.Id;
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException($"Error deserializing JSON response:\n{responseContent}", ex);
        }
    }


    private static string ExtractCpfNumbers(string cpf)
    {
        return Regex.Replace(cpf, @"\D", "");
    }
}

public class CreateCustomerResponse
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = default!;
}