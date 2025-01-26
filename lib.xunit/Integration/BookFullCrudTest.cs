using lib.api.Data;
using lib.api.Models;
using lib.api.Services;

namespace lib.xunit.Integration;

public class BookFullCrudTest : IClassFixture<DbFixture>
{
    private readonly LibService _libService;

    public BookFullCrudTest(DbFixture fixture)
    {
        var dbContext = new AppDbContext(fixture.Options);
        _libService = new LibService(dbContext);
    }

    [Fact]
    public async Task BookCrudSuccess()
    {
        const int id = 33;
        var book = new Book("Pat a Mat", "Donald Duck") { Id = id };

        var create = await _libService.AddBookAsync(book);
        Assert.Equal(1, create);

        var getBook = await _libService.GetBookAsync(id);
        Assert.Equal(book, getBook);

        var update = await _libService.UpdateBookAsync(book, "Bonnie and Clyde", "Mickey Mouse");
        Assert.Equal(1, update);

        var retrieveBook = await _libService.FindEntityAsync<Book>(id);
        var delete = await _libService.DeleteBookAsync(retrieveBook!);
        Assert.Equal(1, delete);
    }
}