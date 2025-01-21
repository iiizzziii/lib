using System.ComponentModel.DataAnnotations;

namespace lib.api.Models;

public class User
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    public ICollection<Borrowing> Borrowings { get; set; }
}