using lib.api.Models;

namespace lib.api.Services;

public interface ILibService
{
    Task<T?> FindEntityAsync<T>(int id) where T : class;
    Task<Book?> GetBookAsync(int id);
    Task<int> AddBookAsync(Book book);
    Task<int> UpdateBookAsync(Book book, string author, string title);
    Task<int> DeleteBookAsync(Book book);
    Task<int> ReturnBookAsync(int id);
    Task<string> BorrowBookAsync(BorrowRequest request);
}