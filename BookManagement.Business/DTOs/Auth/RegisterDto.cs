namespace BookManagement.Business.DTOs.Auth;

public class RegisterDto
{
    public bool Succeeded { get; set; }
    public IEnumerable<string> Errors { get; set; } = [];
}
