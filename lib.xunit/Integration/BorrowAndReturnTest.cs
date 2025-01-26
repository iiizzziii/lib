using lib.api.Data;
using lib.api.Models;
using lib.api.Services;
using Microsoft.EntityFrameworkCore;

namespace lib.xunit.Integration;

public class BorrowAndReturnTest : IClassFixture<DbFixture>
{
    private readonly AppDbContext _dbContext;
    private readonly LibService _libService;

    public BorrowAndReturnTest(DbFixture fixture)
    {
        _dbContext = new AppDbContext(fixture.Options);
        _libService = new LibService(_dbContext);
    }

    [Fact]
    public async Task BorrowAndReturnSuccess()
    {
        var user = await _libService.FindEntityAsync<User>(Ids.Id11);
        var book = await _libService.FindEntityAsync<Book>(Ids.Id11);

        var borrow = await _libService.BorrowBookAsync(user!, book!, 7);
        Assert.Equal(2, borrow);

        var getB = await _dbContext.Borrowings.FirstOrDefaultAsync();
        Assert.NotNull(getB);

        var getBorrowing = await _libService.FindBorrowingAsync<Borrowing>(getB.Id);
        Assert.NotNull(getBorrowing);
        Assert.Equal(Status.Borrowed, getBorrowing.Book.Status);
        Assert.Equal(BorrowStatus.Borrowed, getBorrowing.BorrowStatus);
        Assert.Single(getBorrowing.User.Borrowings);

        var returnBooks = await _libService.ReturnBookAsync(getBorrowing);
        Assert.Equal(2, returnBooks);
        
        var getReturnedBooks = await _libService.FindBorrowingAsync<Borrowing>(getB.Id);
        Assert.NotNull(getReturnedBooks);
        Assert.Equal(Status.Available, getBorrowing.Book.Status);
        Assert.Equal(BorrowStatus.Returned, getBorrowing.BorrowStatus);

        var userBorrowList = await _libService.GetUserAsync(Ids.Id11);
        Assert.Empty(userBorrowList!.ActiveBorrowings);
    }
}