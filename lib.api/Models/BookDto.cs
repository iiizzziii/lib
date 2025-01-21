using System.ComponentModel.DataAnnotations;

namespace lib.api.Models;

public record BookDto
{
    [Required]
    public string Title { get; init; }
    
    [Required]
    public string Author { get; init; }
}