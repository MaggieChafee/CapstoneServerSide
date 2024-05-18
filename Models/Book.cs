namespace Books.Models
{
    public class Book
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Summary { get; set; }
        public int NumberOfPages { get; set; }
        public required string ImageUrl {  get; set; }
        public DateTime PubDate { get; set; }
        public ICollection<Shelf>? Shelf { get; set; }
        public ICollection<Author>? Authors { get; set; }
    }
}
