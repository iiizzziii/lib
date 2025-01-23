// using System.Net.Http.Json;
// using lib.api.Data;
// using lib.api.Models;
// using Microsoft.AspNetCore.Mvc.Testing;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.DependencyInjection;
// using Microsoft.VisualStudio.TestPlatform.TestHost;
//
// namespace lib.xunit.Integration;
//
// // [Collection(nameof(HttpClientCollection))]
// public class BookCrud : IClassFixture<WebApplicationFactory<Program>>
// {
//     // private readonly HttpClientFixture _fixture;
//     private readonly HttpClient client;
//     private readonly WebApplicationFactory<Program> _factory;
//
//     public BookCrud(WebApplicationFactory<Program> factory)
//     {
//         // _fixture = fixture;
//         _factory = factory;
//         client = factory.WithWebHostBuilder(builder =>
//         {
//             builder.ConfigureServices(services =>
//             {
//                 // Replace the DbContext with an in-memory database
//                 var descriptor = services.SingleOrDefault(
//                     d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
//                 if (descriptor != null) services.Remove(descriptor);
//
//                 services.AddDbContext<AppDbContext>(options =>
//                     options.UseSqlite("DataSource=:memory:")); // In-memory SQLite
//             });
//         }).CreateClient();
//     }
//
//     [Fact]
//     public async Task Test1()
//     {
//         using (var scope = _factory.Services.CreateScope())
//         {
//             var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
//             
//             await dbContext.Database.OpenConnectionAsync();
//             await dbContext.Database.EnsureCreatedAsync();
//             
//             var newBook = new { Title = "Test Book", Author = "Test Author" };
//     
//             var response = await client.PostAsJsonAsync("api/lib/add", newBook);
//             
//             var createdBook = await response.Content.ReadFromJsonAsync<Book>();
//             Assert.NotNull(createdBook);
//             Assert.NotEqual(0, createdBook.Id);
//         }
//     }
// }