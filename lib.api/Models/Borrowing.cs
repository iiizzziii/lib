using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace lib.api.Models;

public class Borrowing : IValidatableObject
{
    [Key]
    public int Id { get; init; }
    
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

    public BorrowStatus BorrowStatus { get; set; } = BorrowStatus.Borrowed;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (DateCreated > DateTime.Now) {
            yield return new ValidationResult(
                $"{nameof(DateCreated)} cannot be in the future", [nameof(DateCreated)]); }

        if (DateDue <= DateCreated) {
            yield return new ValidationResult(
                $"{nameof(DateDue)} must be after {nameof(DateCreated)}", [nameof(DateDue)]); }
    }
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum BorrowStatus
{
    Borrowed,
    Returned,
    Overdue
}