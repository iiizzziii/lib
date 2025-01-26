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
        try
        {
            var borrow = await libService.BorrowBookAsync(request);
            if (!string.IsNullOrEmpty(borrow)) return NotFound(borrow);
            
            return Ok("book borrowed successfully");
        }
        catch (Exception e) { Console.WriteLine(e); throw; }
    }

    [HttpPut("return/{id:int}")]
    public async Task<IActionResult> Return(int id)
    {
        try
        {
            var returnBook = await libService.ReturnBookAsync(id);
            if (returnBook.Equals(0)) return NotFound("no return");
            
            return Ok("book returned");
        }
        catch (Exception e) { Console.WriteLine(e); throw; }
    }
}