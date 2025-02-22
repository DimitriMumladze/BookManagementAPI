using BookManagement.Business.DTOs;
using BookManagement.Business.DTOs.Book;

namespace BookManagement.Business.Services
{
    public interface IBookService
    {
        BookPagedListDto GetBooksAsync(int pageNumber, int pageSize);
        Task<BookResponseDto<BookDetailsDto>> GetBookByIdAsync(int id);
        Task<BookResponseDto<object>> AddBookAsync(BookCreateDto bookCreateDto);
        Task<BookResponseDto<object>> AddBooksAsync(IEnumerable<BookCreateDto> booksCreateDto);
        Task<BookResponseDto<BookDetailsDto>> UpdateBookAsync(BookUpdateDto bookUpdateDto);
        Task SoftDeleteBookAsync(int id);
        Task SoftDeleteBooksAsync(BookBulkDeleteDto bookBulkDeleteDto);
    }
}
