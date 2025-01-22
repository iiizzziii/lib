using System.ComponentModel.DataAnnotations;

namespace lib.api.Models;

public record BookDto
{
    [Required]
    [StringLength(200)]
    public string Title { get; init; }
    
    [Required]
    [StringLength(50)]
    public string Author { get; init; }
}