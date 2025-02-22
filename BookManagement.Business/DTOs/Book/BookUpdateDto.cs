namespace BookManagement.Business.DTOs.Book
{
    public class BookUpdateDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int PublicationYear { get; set; }
        public string AuthorName { get; set; }
    }
}
