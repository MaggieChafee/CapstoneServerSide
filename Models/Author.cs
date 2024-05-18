namespace Books.Models
{
    public class Author
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public required string LastName { get; set; }
        public string? Biography { get; set; }
        public string? ImageUrl { get; set; }
        public ICollection<Book>? Books { get; set; }
    }
}
