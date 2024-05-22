using Books.Models;

namespace Books.DTOs
{
    public class UpdateReviewDto
    {
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
