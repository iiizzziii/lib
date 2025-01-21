using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace lib.api.Models;

public class Book
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public string Title { get; set; }
    
    [Required]
    public string Author { get; set; }
    
    [Required]
    public Status Status { get; set; }
    
    public Borrowing Borrowing { get; set; }
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Status
{
    Available,
    Borrowed
}