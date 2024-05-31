namespace Books.Models
{
    public class BookShelf
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public Book Book { get; set; }
        public int ShelfId { get; set; }
        public Shelf Shelf { get; set; }
    }
}
