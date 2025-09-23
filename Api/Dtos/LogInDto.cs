using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Api.Dtos;

public record LongInDto
(
    [EmailAddress]
    [Required]
    string Email,

    [Required]
    string Password
);