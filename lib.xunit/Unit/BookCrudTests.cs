using lib.api.Data;
using lib.api.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace lib.xunit.Unit;

// [Collection(nameof(HttpClientCollection))]
public class BookCrudTests(
    // HttpClientFixture httpFixture
    DbFixture dbFixture
    ) : IClassFixture<DbFixture>//, IClassFixture<HttpClientFixture>
{
    // private readonly HttpClient _httpClient = httpFixture.HttpClient;
    private readonly AppDbContext _dbContext = new(dbFixture.Options);

    [Fact]
    public async Task GetBookSuccess()
    {
        var controller = new LibController(_dbContext);
        var response = await controller.GetBook(22);

        Assert.IsType<OkObjectResult>(response);
    }

    [Theory]
    [InvalidItemIds]
    public async Task GetBookNotFound(int id)
    {
        var controller = new LibController(_dbContext);
        var response = await controller.GetBook(id);

        Assert.IsType<NotFoundObjectResult>(response);
    }
    
    [Fact]
    public async Task DeleteBookSuccess()
    {
        var controller = new LibController(_dbContext);
        var response = await controller.DeleteBook(22);
        var tryGetDeleted = await controller.GetBook(22);
        
        Assert.IsType<OkObjectResult>(response);
        Assert.IsType<NotFoundObjectResult>(tryGetDeleted);
    }
}