using lib.api.Models;
using lib.api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace lib.api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LibController(ILibService libService) : ControllerBase
{
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetBook(int id)
    {
        var book = await libService.GetBookAsync(id);
        if (book == null) return NotFound($"book with id: {id} not found");

        return Ok(book);
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddBook(
        [FromBody] BookDto book)
    {
        var newBook = new Book {
            Title = book.Title,
            Author = book.Author };

        try
        {
            var add = await libService.AddBookAsync(newBook);
            if (add.Equals(0)) return NotFound("book not added");
            
            return Ok($"{newBook.Title} by {newBook.Author} added successfully");
        }
        catch (Exception e) { Console.WriteLine(e); throw; }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateBook(
        int id,
        [FromBody] BookDto book)
    {
        var updateBook = await libService.FindEntityAsync<Book>(id);
        if (updateBook == null) return NotFound($"book with id: {id} not found");
        
        try
        {
            var update = await libService.UpdateBookAsync(updateBook, book.Author, book.Title);
            if (update.Equals(0)) return NotFound("no update");
            
            return Ok($"book {id} updated");
        }
        catch (DbUpdateConcurrencyException)
        {
            return NotFound("no update");
        }
        catch (Exception e) { Console.WriteLine(e); throw; }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteBook(int id)
    {
        var deleteBook = await libService.FindEntityAsync<Book>(id);
        if (deleteBook == null) return NotFound($"book with id: {id} not found");
        
        try
        {
            var delete = await libService.DeleteBookAsync(deleteBook);
            if (delete.Equals(0)) return NotFound("no delete");
            
            return Ok($"book {id} deleted successfully");
        }
        catch (DbUpdateConcurrencyException)
        {
            return NotFound("no delete");
        }
        catch (Exception e) { Console.WriteLine(e); throw; }
    }

    [HttpPost("borrow")]
    public async Task<IActionResult> Borrow(
        [FromBody] BorrowRequest request)
    {
        var user = await libService.FindEntityAsync<User>(request.UserId);
        if (user == null) return NotFound($"user with id: {request.UserId} not found");
        
        var book = await libService.FindEntityAsync<Book>(request.BookId);
        if (book == null) return NotFound($"book with id: {request.BookId} not found");
        if (book.Status == Status.Borrowed) return Conflict($"book with id: {request.BookId} is borrowed");
        
        try
        {
            var borrow = await libService.BorrowBookAsync(user, book, request.Duration);
            if (borrow.Equals(0)) return NotFound("book not borrowed");
            
            return Ok("book borrowed successfully");
        }
        catch (Exception e) { Console.WriteLine(e); throw; }
    }

    [HttpPut("return/{id:int}")]
    public async Task<IActionResult> Return(int id)
    {
        var borrowing = await libService.FindBorrowingAsync<Borrowing>(id);
        if (borrowing == null) return NotFound($"borrowing with id: {id} not found");
        if (borrowing.BorrowStatus == BorrowStatus.Returned) return Conflict($"already returned");
        
        try
        {
            var returnBook = await libService.ReturnBookAsync(borrowing);
            if (returnBook.Equals(0)) return NotFound("no return");
            
            return Ok("book returned");
        }
        catch (Exception e) { Console.WriteLine(e); throw; }
    }
    
    [HttpGet("user/{id:int}")]
    public async Task<IActionResult> GetUser(int id)
    {
        var user = await libService.GetUserAsync(id);
        if (user == null) return NotFound($"user with id: {id} not found");
        
        return Ok(user);
    }
}