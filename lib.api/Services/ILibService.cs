using lib.api.Models;

namespace lib.api.Services;

public interface ILibService
{
    Task<T?> FindEntityAsync<T>(int id) where T : class;
    Task<T?> FindBorrowingAsync<T>(int id) where T : Borrowing;
    Task<Book?> GetBookAsync(int id);
    Task<int> AddBookAsync(Book book);
    Task<int> UpdateBookAsync(Book book, string author, string title);
    Task<int> DeleteBookAsync(Book book);
    Task<int> BorrowBookAsync(User user, Book book, int duration);
    Task<int> ReturnBookAsync(Borrowing borrowing);
    Task<UserDto?> GetUserAsync(int id);
}