namespace Books.Controllers
{
    public class SearchApi
    {
        public static void Map(WebApplication app)
        {
            app.MapGet("/books/search/{query}", (BooksDbContext db, string query) => 
            {

                var filteredBooks = db.Books
                    .Where(b => b.Title.ToLower().Contains(query.ToLower())) 
                    .Select(b => new
                    {
                        b.Id,
                        b.Title,
                        b.ImageUrl
                    })
                    .ToList();

                if (filteredBooks.Count == 0)
                {
                    return Results.NotFound("no books");
                }

                return Results.Ok(filteredBooks);

            });
        }
    }
}
