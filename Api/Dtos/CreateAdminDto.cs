using System.ComponentModel.DataAnnotations;

namespace Api.Dtos;

public record CreateAdminDto
(
    [Required]
    string Name,

    [Required]
    [EmailAddress]
    string Email,

    [Required]
    [MinLength(8)]
    string Password
);