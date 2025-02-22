using BookManagement.Data.Models;
using BookManagement.Data.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BookManagement.Data.Repositories;

public class BookRepository : IBookRepository
{
    private readonly BookManagementDbContext _context;

    public BookRepository(BookManagementDbContext context)
    {
        _context = context;
    }

    public IQueryable<Book> GetBooksAsync()
    {
        return _context.Books
            .OrderByDescending(b => b.ViewsCount)
            .Where(b => !b.IsDeleted)
            .AsQueryable();
    }

    public async Task<Book?> GetBookByIdAsync(int id)
    {
        return await _context.Books.FirstOrDefaultAsync(b => b.Id == id && !b.IsDeleted);
    }

    public async Task<Book?> GetBookByTitleAsync(string title)
    {
        return await _context.Books.FirstOrDefaultAsync(b => b.Title == title && !b.IsDeleted);
    }

    public async Task AddBookAsync(Book book)
    {
        _context.Books.Add(book);
        await _context.SaveChangesAsync();
    }

    public async Task AddBooksAsync(IEnumerable<Book> books)
    {
        _context.Books.AddRange(books);
        await _context.SaveChangesAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task SoftDeleteBookAsync(int id)
    {
        var book = await GetBookByIdAsync(id);
        if (book != null)
        {   
            book.IsDeleted = true;
            await _context.SaveChangesAsync();
        }
    }

    public async Task SoftDeleteBooksAsync(IEnumerable<int> ids)
    {
        var books = await _context.Books.Where(b => ids.Contains(b.Id)).ToListAsync();
        books.ForEach(book => book.IsDeleted = true);
        await _context.SaveChangesAsync();
    }
}

