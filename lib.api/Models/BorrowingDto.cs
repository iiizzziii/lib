namespace lib.api.Models;

public class BorrowingDto
{
    public string BookTitle { get; set; }
    public string BookAuthor { get; set; }
    public DateTime DateDue { get; set; }
}