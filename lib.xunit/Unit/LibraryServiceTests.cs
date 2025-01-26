using lib.api.Data;
using lib.api.Models;
using lib.api.Services;
using Microsoft.EntityFrameworkCore;

namespace lib.xunit.Unit;

public class LibraryServiceTests : IClassFixture<DbFixture>
{
    private readonly AppDbContext _dbContext;
    private readonly LibService _libService;

    public LibraryServiceTests(DbFixture fixture)
    {
        _dbContext = new AppDbContext(fixture.Options);
        _libService = new LibService(_dbContext);
    }
    
    [Fact]
    public async Task AddBookAsyncSuccess()
    {
        var book = new Book("Test Book 369", "Test Author 369");

        var result = await _libService.AddBookAsync(book);
        
        Assert.Equal(1, result);
        var bookDb = await _dbContext.Books.FirstOrDefaultAsync(b => b.Id == book.Id);
        Assert.NotNull(bookDb);
        Assert.Equal(book.Author, bookDb.Author);
        Assert.Equal(book.Title, bookDb.Title);
    }
    
    [Fact]
    public async Task AddBookAsyncFailure()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(() => _libService.AddBookAsync(null!));
    }

    [Fact]
    public async Task FindEntityAsyncSuccess()
    {
        var result = await _libService.FindEntityAsync<User>(Ids.Id11);

        Assert.NotNull(result);
    }
    
    [Fact]
    public async Task FindEntityAsyncNotFound()
    {
        var result = await _libService.FindEntityAsync<Borrowing>(Ids.Id11);
        
        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateBookAsyncNoUpdateForSameProperties()
    {
        const string title = "Test Book 1";
        const string author = "Test Author 1";
        
        var result = await _libService.UpdateBookAsync(
            new Book(title, author) { Id = Ids.Id11 }, 
            author, 
            title);
        
        Assert.Equal(0, result);
    }
    
    [Fact]
    public async Task BorrowBookAsyncSuccess()
    {
        var user = await _libService.FindEntityAsync<User>(Ids.Id11);
        var book = await _libService.FindEntityAsync<Book>(Ids.Id11);
        var result = await _libService.BorrowBookAsync(user!, book!, Ids.Id11);
        
        Assert.Equal(2, //new borrowing + book status update
            result);
    }
    
    [Fact]
    public async Task ReturnBookAsyncSuccess()
    {
        var user = await _libService.FindEntityAsync<User>(Ids.Id11);
        var book = await _libService.FindEntityAsync<Book>(Ids.Id11);
        var borrowing = new Borrowing
        {
            User = user!, 
            Book = book!, 
            DateCreated = DateTime.Now, 
            DateDue = DateTime.Now.AddDays(7)
        };
        await _dbContext.Borrowings.AddAsync(borrowing);
        
        var result = await _libService.ReturnBookAsync(borrowing);
        
        Assert.Equal(2, //borrowing update + book status update
            result);
        Assert.Equal(Status.Available, borrowing.Book.Status);
        Assert.Equal(BorrowStatus.Returned, borrowing.BorrowStatus);
    }
}