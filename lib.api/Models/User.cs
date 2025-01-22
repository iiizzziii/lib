using System.ComponentModel.DataAnnotations;

namespace lib.api.Models;

public class User
{
    [Key]
    public int Id { get; init; }
    
    [Required]
    [EmailAddress]
    [StringLength(50)]
    public string Email { get; set; }
    
    public ICollection<Borrowing> Borrowings { get; set; }
}