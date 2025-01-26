namespace lib.api.Models;

public class UserDto
{
    public int Id { get; set; }
    public string Email { get; set; }
    public List<BorrowingDto> ActiveBorrowings { get; set; }
}