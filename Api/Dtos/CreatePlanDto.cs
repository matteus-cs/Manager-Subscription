using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Api.Dtos;

public record CreatePlanDto
(
    [Required]
    [StringLength(100, MinimumLength = 3)]
    string Name,

    [StringLength(255, MinimumLength = 3)]
    string? Description,

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero")]  
    decimal MonthlyPrice 
);