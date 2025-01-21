using System.ComponentModel.DataAnnotations;

namespace lib.api.Models;

public record BorrowRequest
{
    [Required]
    public int UserId { get; init; }
    
    [Required]
    public int BookId { get; init; }

    public int Duration { get; init; }
}