using Api.Entities;
using System.ComponentModel.DataAnnotations;

namespace Api.Dtos;

public record CreateCustomerDto
(
    [Required]
    [StringLength(50, MinimumLength = 2)]
    string Name,

    [Required]
    [EmailAddress]
    string Email,

    [Required]
    [StringLength(14, MinimumLength = 11)]
    string Cpf,

    [Required]
    [RegularExpression(@"^(?:\+55\s?)?\(?\d{2}\)?\s?(?:9\d{4}|\d{4})-?\d{4}$", ErrorMessage = "Invalid phone format")]
    string Phone,

    [Required]
    CreateAddressDto Address,

    [Required]
    [MinLength(8)]
    string Password
);

public record CreateAddressDto
(
    [Required]
    string Street,

    [Required]
    string Neighborhood,

    [Required]
    string City,

    [Required]
    string State
);