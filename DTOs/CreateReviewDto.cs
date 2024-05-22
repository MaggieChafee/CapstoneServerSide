using Books.Models;

namespace Books.DTOs
{
    public class CreateReviewDto
    {
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime DateCreated { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
    }
}
