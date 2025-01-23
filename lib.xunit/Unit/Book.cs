using lib.api.Controllers;
using lib.api.Data;
using lib.api.Models;
using Microsoft.AspNetCore.Mvc;

namespace lib.xunit.Unit;

public class BookT : IClassFixture<DbFixture>
{
    private readonly DbFixture _fixture;
    private readonly AppDbContext _dbContext;

    public BookT(DbFixture fixture)
    {
        _fixture = fixture;
        _dbContext = new AppDbContext(_fixture.Options);
    }
    
    [Fact]
    public async Task Test2()
    {
        var controller = new LibController(_dbContext);
        var book = await controller.GetBook(11);
        var ok = book as OkObjectResult;
        var bObject = ok.Value as Book;
        Assert.Equal(11, bObject.Id);
        
        // Book bObject;
        //
        // await using (var dbContext = new AppDbContext(fixture.Options))
        // {
        //     var controller = new LibController(dbContext);
        //     var book = await controller.GetBook(1);
        //     var ok = book as OkObjectResult;
        //     bObject = ok.Value as Book;
        //     Assert.Equal(1, );
        // }
    }
}