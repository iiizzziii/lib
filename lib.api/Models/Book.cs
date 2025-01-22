using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace lib.api.Models;

public class Book
{
    [Key]
    public int Id { get; init; }
    
    [Required]
    [StringLength(200)]
    public string Title { get; set; }
    
    [Required]
    [StringLength(50)]
    public string Author { get; set; }

    [Required] 
    public Status Status { get; set; } = Status.Available;
    
    public Borrowing Borrowing { get; set; }
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Status
{
    Available,
    Borrowed
}