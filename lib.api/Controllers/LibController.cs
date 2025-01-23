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
        catch (DbUpdateConcurrencyException)
        {
            return NotFound($"book with id: {id} not found");
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

        try
        {
            var newBorrowing = new Borrowing
            {
                UserId = request.UserId,
                BookId = request.BookId,
                DateCreated = DateTime.Now,
                DateDue = DateTime.Today.AddDays(request.Duration),
            };
            book.Status = Status.Borrowed;
            
            await dbContext.Borrowings.AddAsync(newBorrowing);
            await dbContext.SaveChangesAsync();

            return Ok($"user {user.Email} borrowed {book.Title} until {newBorrowing.DateDue}");
        }
        catch (Exception e) { Console.WriteLine(e); throw; }
    }

    [HttpPut("return/{id:int}")]
    public async Task<IActionResult> Return(int id)
    {
        var borrowing = await dbContext.Borrowings
            .Include(b => b.Book)
            .FirstOrDefaultAsync(b => b.Id == id);
        if (borrowing == null) return NotFound($"borrowing with id: {id} not found");

        try
        {
            borrowing.Book.Status = Status.Available;
            borrowing.BorrowStatus = BorrowStatus.Returned;
            await dbContext.SaveChangesAsync();

            return Ok($"{borrowing.Book.Title} returned");
        }
        catch (Exception e) { Console.WriteLine(e); throw; }
    }
}