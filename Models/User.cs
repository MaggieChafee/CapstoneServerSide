namespace Books.Models
{
    public class User
    {
        public int Id { get; set; }
        public required string Username { get; set; }
        public required string Uid { get; set; }
        public string? Email { get; set; }
        public string? ImageUrl { get; set; }
    }
}
