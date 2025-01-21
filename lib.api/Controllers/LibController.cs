using lib.api.Data;
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
}