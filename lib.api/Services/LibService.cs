using lib.api.Data;
using lib.api.Models;
using Microsoft.EntityFrameworkCore;

namespace lib.api.Services;

public class LibService(AppDbContext dbContext) : ILibService
{
    public async Task<T?> FindEntityAsync<T>(int id) where T : class
    {
        return await dbContext.Set<T>().FindAsync(id);
    }

    public async Task<T?> FindBorrowingAsync<T>(int id) where T : Borrowing
    {
        return await dbContext.Set<Borrowing>()
            .Include(b => b.Book)
            .Include(b => b.User)
            .FirstOrDefaultAsync(e => e.Id == id) as T;
    }
    
    public async Task<Book?> GetBookAsync(int id)
    {
        return await dbContext.Books.FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<int> AddBookAsync(Book book)
    {
        await dbContext.Books.AddAsync(book);
        
        return await dbContext.SaveChangesAsync();
    }

    public async Task<int> UpdateBookAsync(Book book, string author, string title)
    {
        book.Author = author;
        book.Title = title;

        if (!dbContext.Entry(book).Properties.Any(p => p.IsModified)) return 0;
        
        return await dbContext.SaveChangesAsync();
    }

    public async Task<int> DeleteBookAsync(Book book)
    {
        dbContext.Books.Remove(book);
        
        return await dbContext.SaveChangesAsync();
    }

    public async Task<int> BorrowBookAsync(User user, Book book, int duration)
    {
        var newBorrowing = new Borrowing 
        {
            User = user,
            Book = book,
            UserId = user.Id,
            BookId = book.Id,
            DateCreated = DateTime.Now,
            DateDue = DateTime.Today.AddDays(duration)
        };
        book.Status = Status.Borrowed;
        
        await dbContext.Borrowings.AddAsync(newBorrowing);
        
        return await dbContext.SaveChangesAsync();
    }

    public async Task<int> ReturnBookAsync(Borrowing borrowing)
    {
        borrowing.Book.Status = Status.Available;
        borrowing.BorrowStatus = BorrowStatus.Returned;
        
        return await dbContext.SaveChangesAsync();
    }
    
    public async Task<UserDto?> GetUserAsync(int id)
    {
        return await dbContext.Users
            .Where(u => u.Id == id)
            .Select(u => new UserDto
            {
                Id = u.Id,
                Email = u.Email,
                ActiveBorrowings = u.Borrowings
                    .Where(b => b.BorrowStatus == BorrowStatus.Borrowed)
                    .Select(b => new BorrowingDto {
                        BookTitle = b.Book.Title,
                        BookAuthor = b.Book.Author,
                        DateDue = b.DateDue }).ToList() }).FirstOrDefaultAsync();
    }
}