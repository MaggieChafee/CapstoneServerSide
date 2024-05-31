namespace Books.Models
{
    public class Shelf
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public required string Name {  get; set; }
        public List<BookShelf>? BookShelves { get; set; }
    }
}
