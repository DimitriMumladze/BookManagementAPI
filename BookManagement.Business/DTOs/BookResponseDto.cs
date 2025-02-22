namespace BookManagement.Business.DTOs;

public class BookResponseDto<T>
{
    public T? Data { get; set; }
    public int StatusCode { get; set; }
    public string Message { get; set; } = string.Empty;
}
