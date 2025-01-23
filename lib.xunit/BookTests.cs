using lib.api.Controllers;
using lib.api.Data;
using lib.api.Models;
using Microsoft.AspNetCore.Mvc;

namespace lib.xunit;

public class BookTests(DbFixture fixture) : IClassFixture<DbFixture>
{
    private readonly AppDbContext _dbContext = new(fixture.Options);

    [Fact]
    public async Task Test2()
    {
        var controller = new LibController(_dbContext);
        var book = await controller.GetBook(11);
        var ok = book as OkObjectResult;
        var bObject = ok.Value as Book;
        Assert.Equal(11, bObject.Id);
    }

    [Fact]
    public async Task Test3()
    {
        var controller = new LibController(_dbContext);

        var newBook = new BookDto
        {
            Title = "book",
            Author = "author",
        };

        var response = await controller.AddBook(newBook);

        Assert.IsType<OkObjectResult>(response);
    }
}