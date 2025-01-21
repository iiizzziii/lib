using lib.api.Data;
using lib.api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace lib.api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LibController(AppDbContext dbContext) : ControllerBase
{
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetBook(int id)
    {
        var book = await dbContext.Books.FirstOrDefaultAsync(b => b.Id == id);
        if (book == null) return NotFound($"book with id: {id} not found");

        return Ok(book);
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddBook(
        [FromBody] BookDto book)
    {
        var newBook = new Book
        {
            Title = book.Title,
            Author = book.Author,
        };

        try
        {
            await dbContext.Books.AddAsync(newBook);
            await dbContext.SaveChangesAsync();

            return Ok($"{newBook.Title} by {newBook.Author} added successfully");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateBook(
        int id,
        [FromBody] BookDto book)
    {
        var bookUpdate = await dbContext.Books.FirstOrDefaultAsync(b => b.Id == id);
        if (bookUpdate == null) return NotFound($"book with id: {id} not found");

        bookUpdate.Title = book.Title;
        bookUpdate.Author = book.Author;

        try
        {
            await dbContext.SaveChangesAsync();

            return Ok($"book {id} updated");
        }
        catch (Exception e) { Console.WriteLine(e); throw; }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteBook(int id)
    {
        try
        {
            var book = new Book { Id = id };
            dbContext.Books.Attach(book);
            dbContext.Books.Remove(book);
            await dbContext.SaveChangesAsync();

            return Ok($"book {id} deleted successfully");
        }
        catch (Exception e) { Console.WriteLine(e); throw; }
    }

    [HttpPost("borrow")]
    public async Task<IActionResult> Borrow(
        [FromBody] BorrowRequest request)
    {
        var book = await dbContext.Books.FindAsync(request.BookId);
        if (book == null) return NotFound($"book with id: {request.BookId} not found");
        if (book.Status == Status.Borrowed) return BadRequest($"{book.Title} is borrowed");

        var user = await dbContext.Users.FindAsync(request.UserId);
        if (user == null) return NotFound($"user with id: {request.UserId} not found");

        var newBorrowing = new Borrowing
        {
            UserId = request.UserId,
            BookId = request.BookId,
            DateCreated = DateTime.Now,
            DateDue = DateTime.Today.AddDays(request.Duration),
        };

        await dbContext.Borrowings.AddAsync(newBorrowing);
        await dbContext.SaveChangesAsync();

        return Ok($"{user.Email} borrowed {book.Title} until {newBorrowing.DateDue}");
    }
}