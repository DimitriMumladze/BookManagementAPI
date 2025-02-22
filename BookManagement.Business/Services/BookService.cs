using BookManagement.Business.DTOs.Book;
using BookManagement.Data.Models;
using BookManagement.Data.Repositories;
using BookManagement.Business.DTOs;



namespace BookManagement.Business.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;

        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public BookPagedListDto GetBooksAsync(int pageNumber, int pageSize)
        {
            var books = _bookRepository.GetBooksAsync();

            var paginatedBooks = books
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            int totalCount = books.Count(); 
            int totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            var bookTitles = paginatedBooks.Select(b => new BookTitleDto
            {
                Id = b.Id,
                Title = b.Title
            }).ToList();

            return new BookPagedListDto
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalPages,
                Books = bookTitles
            };
        }

        public async Task<BookResponseDto<BookDetailsDto>> GetBookByIdAsync(int id)
        {
            var book = await _bookRepository.GetBookByIdAsync(id);
            if (book == null)
            {
                return new BookResponseDto<BookDetailsDto>()
                {
                    StatusCode = 404,
                    Data = new BookDetailsDto(),
                    Message = "Book not found"
                };
            }

            int yearsSincePublished = DateTime.Now.Year - book.PublicationYear;
            double popularityScore = book.ViewsCount * 0.5 + yearsSincePublished * 2;

            book.ViewsCount += 1;

            await _bookRepository.SaveChangesAsync();

            var bookDetailsDto =  new BookDetailsDto
            {
                Id = book.Id,
                Title = book.Title,
                PublicationYear = book.PublicationYear,
                AuthorName = book.AuthorName,
                ViewsCount = book.ViewsCount,
                PopularityScore = popularityScore
            };

            return new BookResponseDto<BookDetailsDto>()
            {
                Data = bookDetailsDto,
                StatusCode = 200,
                Message = "Books retrived successfully"
            };
        }

        public async Task<BookResponseDto<object>> AddBookAsync(BookCreateDto bookCreateDto)
        {
            var validateWithTitle = await _bookRepository.GetBookByTitleAsync(bookCreateDto.Title);
            if (validateWithTitle != null)
                return new BookResponseDto<object>()
                {
                    StatusCode = 409,
                    Message = "book with this title already exsits"
                };

            var newBook = new Book
            {
                Title = bookCreateDto.Title,
                PublicationYear = bookCreateDto.PublicationYear,
                AuthorName = bookCreateDto.AuthorName,
            };

            await _bookRepository.AddBookAsync(newBook);

            return new BookResponseDto<object>()
            {
                StatusCode = 200,
                Message = "Book added successfully"
            };
        }

        public async Task<BookResponseDto<object>> AddBooksAsync(IEnumerable<BookCreateDto> booksCreateDto)
        {
            var validateWithTitle = booksCreateDto.Select(b => _bookRepository.GetBookByTitleAsync(b.Title));
            if (!validateWithTitle.Any())
            {
                return new BookResponseDto<object>()
                {
                    StatusCode = 409,
                    Message = "book with this title already exsits"
                };
            }

            var newBooks = booksCreateDto.Select(book => new Book
            {
                Title = book.Title,
                PublicationYear = book.PublicationYear,
                AuthorName = book.AuthorName,
            });

            await _bookRepository.AddBooksAsync(newBooks);

            return new BookResponseDto<object>()
            {
                StatusCode = 200,
                Message = "Book added successfully"
            };
        }

        public async Task<BookResponseDto<BookDetailsDto>> UpdateBookAsync(BookUpdateDto bookUpdateDto)
        {
            var book = await _bookRepository.GetBookByIdAsync(bookUpdateDto.Id);
            if (book == null)
            {
                return new BookResponseDto<BookDetailsDto>()
                {
                    StatusCode = 404,
                    Data = new BookDetailsDto(),
                    Message = "Book not found"
                };
            }

            if (!string.Equals(book.Title, bookUpdateDto.Title, StringComparison.OrdinalIgnoreCase))
            {
                var existingBook = await _bookRepository.GetBookByTitleAsync(bookUpdateDto.Title);
                if (existingBook != null && existingBook.Id != book.Id)
                {
                    return new BookResponseDto<BookDetailsDto>()
                    {
                        StatusCode = 409,
                        Message = "book with this title already exsits",
                        Data = new BookDetailsDto(),
                    };
                }
            }

            book.Title = bookUpdateDto.Title;
            book.PublicationYear = bookUpdateDto.PublicationYear;
            book.AuthorName = bookUpdateDto.AuthorName;

            await _bookRepository.SaveChangesAsync();

            var bookDetailsDto = new BookDetailsDto
            {
                Id = book.Id,
                Title = book.Title,
                PublicationYear = book.PublicationYear,
                AuthorName = book.AuthorName,
                ViewsCount = book.ViewsCount
            };

            return new BookResponseDto<BookDetailsDto>()
            {
                Data = bookDetailsDto,
                StatusCode = 200,
                Message = "Book updated successfully"
            };
        }

        public async Task SoftDeleteBookAsync(int id)
        {
            await _bookRepository.SoftDeleteBookAsync(id);
        }

        public async Task SoftDeleteBooksAsync(BookBulkDeleteDto bookBulkDeleteDto)
        {
            await _bookRepository.SoftDeleteBooksAsync(bookBulkDeleteDto.BookIds);
        }
    }
}
