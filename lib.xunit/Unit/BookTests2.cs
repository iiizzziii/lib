// using lib.api.Controllers;
// using lib.api.Data;
// using lib.api.Models;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore.Diagnostics;
// using NSubstitute;
//
// namespace lib.xunit.Unit;
//
// public class BookTests
// {
//     private readonly DbContextOptions<AppDbContext> _contextOptions;
//
//     public BookTests()
//     {
//         _contextOptions = new DbContextOptionsBuilder<AppDbContext>()
//             .UseInMemoryDatabase("db")
//             .ConfigureWarnings(d => d.Ignore(InMemoryEventId.TransactionIgnoredWarning))
//             .Options;
//
//         // ReSharper disable once ConvertToUsingDeclaration
//         using (var dbContext = new AppDbContext(_contextOptions))
//         {
//             dbContext.Database.EnsureDeleted();
//             dbContext.Database.EnsureCreated();
//             
//             dbContext.AddRange(
//                 new Book { Id = 11, Title = "Test Book 1", Author = "Test Author 1"},
//                 new Book { Id = 22, Title = "Test Book 2", Author = "Test Author 2"});
//
//             dbContext.SaveChanges();
//         }
//     }
//
//     [Fact]
//     public async Task Test1()
//     {
//         await using var dbContext = new AppDbContext(_contextOptions);
//         var controller = new LibController(dbContext);
//
//         var book = await controller.GetBook(11);
//         var ok = book as OkObjectResult;
//         var bookObject = ok.Value as Book;
//         
//         Assert.Equal(11, bookObject.Id);
//     }
// }

// public class LibControllerTests
// {
//     private readonly AppDbContext _mockDbContext;
//     private readonly LibController _controller;
//
//     public LibControllerTests()
//     {
//         _mockDbContext = Substitute.For<AppDbContext>();
//         _controller = new LibController(_mockDbContext);
//     }
//
//     [Fact]
//     public async Task GetBook_ShouldReturnBook_WhenBookExists()
//     {
//         // Arrange
//         var book = new Book { Id = 1, Title = "Book 1", Author = "Author 1", Status = Status.Available };
//         var books = new List<Book> { book }.AsQueryable();
//         _mockDbContext.Books.Returns(books);
//
//         // Act
//         var result = await _controller.GetBook(1);
//
//         // Assert
//         var okResult = Assert.IsType<OkObjectResult>(result);
//         var returnedBook = Assert.IsType<Book>(okResult.Value);
//         Assert.Equal("Book 1", returnedBook.Title);
//     }
// }
