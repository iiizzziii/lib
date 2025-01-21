using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lib.api.Models;

public class Borrowing
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public int UserId { get; set; }
    
    [Required]
    public int BookId { get; set; }
    
    [Required]
    public DateTime DateCreated { get; set; }
    
    [Required]
    public DateTime DateDue { get; set; }
    
    [ForeignKey(nameof(UserId))]
    public User User { get; set; }
    
    [ForeignKey(nameof(BookId))]
    public Book Book { get; set; }
}