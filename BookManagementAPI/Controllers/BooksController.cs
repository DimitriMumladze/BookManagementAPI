using Microsoft.AspNetCore.Mvc;
using BookManagement.Business.Services;
using Microsoft.AspNetCore.Authorization;
using BookManagement.Business.DTOs.Book;

namespace BookManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        public IActionResult GetBooks([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = _bookService.GetBooksAsync(pageNumber, pageSize);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookById(int id)
        {
            var result = await _bookService.GetBookByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateBook([FromBody] BookCreateDto bookCreateDto)
        {
            try
            {
                var result = await _bookService.AddBookAsync(bookCreateDto);
                return StatusCode(result.StatusCode, result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("bulk")]
        [Authorize]
        public async Task<IActionResult> CreateBooksBulk([FromBody] IEnumerable<BookCreateDto> booksCreateDto)
        {
            try
            {
                var result = await _bookService.AddBooksAsync(booksCreateDto);
                return StatusCode(result.StatusCode, result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateBook([FromRoute] int id, [FromBody] BookUpdateDto bookUpdateDto)
        {
            try
            {
                bookUpdateDto.Id = id;
                var result = await _bookService.UpdateBookAsync(bookUpdateDto);
                return StatusCode(result.StatusCode, result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> SoftDeleteBook(int id)
        {
            try
            {
                await _bookService.SoftDeleteBookAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("bulk")]
        [Authorize]
        public async Task<IActionResult> SoftDeleteBooks([FromBody] BookBulkDeleteDto bookBulkDeleteDto)
        {
            try
            {
                await _bookService.SoftDeleteBooksAsync(bookBulkDeleteDto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
