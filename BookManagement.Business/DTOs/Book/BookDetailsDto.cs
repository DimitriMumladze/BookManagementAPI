﻿namespace BookManagement.Business.DTOs.Book
{
    public class BookDetailsDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int PublicationYear { get; set; }
        public string AuthorName { get; set; }
        public int ViewsCount { get; set; }
        public double PopularityScore { get; set; }
    }
}
