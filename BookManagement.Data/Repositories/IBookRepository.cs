using BookManagement.Data.Models;

namespace BookManagement.Data.Repositories;

public interface IBookRepository
{
    IQueryable<Book> GetBooksAsync();
    Task<Book?> GetBookByIdAsync(int id);
    Task<Book?> GetBookByTitleAsync(string title);
    Task AddBookAsync(Book book);
    Task AddBooksAsync(IEnumerable<Book> books);
    Task SaveChangesAsync();
    Task SoftDeleteBookAsync(int id);
    Task SoftDeleteBooksAsync(IEnumerable<int> ids);
}
