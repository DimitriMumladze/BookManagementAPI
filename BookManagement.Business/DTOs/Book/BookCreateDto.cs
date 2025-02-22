namespace BookManagement.Business.DTOs.Book
{
    public class BookCreateDto
    {
        public required string Title { get; set; }
        public required int PublicationYear { get; set; }
        public required string AuthorName { get; set; }
    }
}
