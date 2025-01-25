using lib.api.Models;
using System.ComponentModel.DataAnnotations;

namespace lib.xunit.Unit;

public class ModelTests
{
    [Fact]
    public void CreateBookSuccess()
    {
        var book = new Book {
            Title = "Tester book",
            Author = "Johnny Test" };

        Assert.NotNull(book);
        Assert.NotNull(book.Author);
        Assert.NotNull(book.Title);
        Assert.IsType<int>(book.Id);
        Assert.Equal(Status.Available, book.Status);
        Assert.Null(book.Borrowings);
    }

    [Theory]
    [InvalidBookDtoParameters]
    public void CreateBookDtoValidationFailure(string author, string title)
    {
        var book = new BookDto {
            Author = author,
            Title = title };

        var validationContext = new ValidationContext(book);
        var validationResult = new List<ValidationResult>();

        Assert.False(Validator.TryValidateObject(book, validationContext, validationResult, true));
    }

    [Fact]
    public void CreateBorrowingValidationSuccess()
    {
        var borrowing = new Borrowing {
            UserId = 1,
            BookId = 1,
            DateCreated = DateTime.Now.AddDays(-1),
            DateDue = DateTime.Now.AddDays(7) };

        var validationContext = new ValidationContext(borrowing);
        var validationResults = borrowing.Validate(validationContext).ToList();

        Assert.Empty(validationResults);
        Assert.True(Validator.TryValidateObject(borrowing, validationContext, validationResults, true));
        Assert.Equal(BorrowStatus.Borrowed, borrowing.BorrowStatus);
    }

    [Fact]
    public void CreateBorrowingDateTimeValidationFailure()
    {
        var borrowing = new Borrowing {
            UserId = 1,
            BookId = 1,
            DateCreated = DateTime.Now.AddDays(1),
            DateDue = DateTime.Now.AddDays(-1) };

        var validationContext = new ValidationContext(borrowing);
        var validationResults = borrowing.Validate(validationContext).ToList();

        Assert.Equal(2, validationResults.Count);
        Assert.Contains(validationResults, r => 
            r.ErrorMessage != null && 
            r.ErrorMessage.Contains("DateCreated cannot be in the future"));
        Assert.Contains(validationResults, r => 
            r.ErrorMessage != null && 
            r.ErrorMessage.Contains("DateDue must be after DateCreated"));
    }
}