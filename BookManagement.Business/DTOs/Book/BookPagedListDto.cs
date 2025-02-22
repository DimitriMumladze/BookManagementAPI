using System.Collections.Generic;

namespace BookManagement.Business.DTOs.Book
{
    public class BookPagedListDto
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }

        public List<BookTitleDto> Books { get; set; }
    }
}
