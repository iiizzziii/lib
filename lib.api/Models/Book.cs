using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace lib.api.Models;

public class Book
{
    [Key]
    public int Id { get; init; }
    public string Title { get; set; }
    public string Author { get; set; }
    public Status Status { get; set; } = Status.Available;
    public ICollection<Borrowing> Borrowings { get; set; }
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Status
{
    Available,
    Borrowed
}
