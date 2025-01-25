using lib.api.Data;
using lib.api.Models;
using Microsoft.EntityFrameworkCore;

namespace lib.api.Services;

public class LibService(AppDbContext dbContext) : ILibService
{
    public async Task<Book?> GetBookAsync(int id)
    {
        return await dbContext.Books.FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<int> AddBookAsync(Book book)
    {
        await dbContext.Books.AddAsync(book);
        return await dbContext.SaveChangesAsync();
    }

    public async Task<int> UpdateBookAsync(int id, string author, string title)
    {
        var book = await dbContext.Books.FindAsync(id);
        if (book == null) return 0;

        book.Author = author;
        book.Title = title;
        
        return await dbContext.SaveChangesAsync();
    }

    public async Task<int> DeleteBookAsync(int id)
    {
        var book = new Book { Id = id };
        dbContext.Books.Attach(book);
        dbContext.Books.Remove(book);
        
        return await dbContext.SaveChangesAsync();
    }

    public async Task<string> BorrowBookAsync(BorrowRequest request)
    {
        var user = await dbContext.Users.FindAsync(request.UserId);
        if (user == null) return $"user with id: {request.UserId} not found";
        
        var book = await dbContext.Books.FindAsync(request.BookId);
        if (book == null) return $"user with id: {request.UserId} not found";
        if (book.Status == Status.Borrowed) return $"{book.Title} is borrowed";
        
        var newBorrowing = new Borrowing {
            UserId = request.UserId,
            BookId = request.BookId,
            DateCreated = DateTime.Now,
            DateDue = DateTime.Today.AddDays(request.Duration) };
        book.Status = Status.Borrowed;
            
        await dbContext.Borrowings.AddAsync(newBorrowing);
        await dbContext.SaveChangesAsync();
        return string.Empty;
    }

    public async Task<int> ReturnBookAsync(int id)
    {
        var borrowing = await dbContext.Borrowings
            .Include(b => b.Book)
            .FirstOrDefaultAsync(b => b.Id == id);
        if (borrowing == null) return 0;
        
        borrowing.Book.Status = Status.Available;
        borrowing.BorrowStatus = BorrowStatus.Returned;
        return await dbContext.SaveChangesAsync();
    }
}