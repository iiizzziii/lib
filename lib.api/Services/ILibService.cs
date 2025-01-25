using lib.api.Models;

namespace lib.api.Services;

public interface ILibService
{
    Task<Book?> GetBookAsync(int id);
    Task<int> AddBookAsync(Book book);
    Task<int> UpdateBookAsync(int id, string author, string title);
    Task<int> DeleteBookAsync(int id);
    Task<int> ReturnBookAsync(int id);
    Task<string> BorrowBookAsync(BorrowRequest request);
}