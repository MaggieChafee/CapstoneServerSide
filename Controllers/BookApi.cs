using System.Reflection.Metadata.Ecma335;
using Books.Controllers;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;

namespace Books.Controllers
{
    public class BookApi
    {
        public static void Map(WebApplication app)
        {
            app.MapGet("/books", (BooksDbContext db) => 
            {
                if (db.Books == null)
                {
                    return Results.Empty;
                }
                return Results.Ok(db.Books);
            });

            app.MapGet("/books/recent-releases", (BooksDbContext db) =>
            {
                var recentReleases = db.Books
                    .OrderByDescending(b => b.PubDate)
                    .Take(25)
                    .ToList();
                if (recentReleases == null)
                {
                    return Results.BadRequest();
                }
                return Results.Ok(recentReleases);
            });

            app.MapGet("/books/{bookId}", (BooksDbContext db, int bookId) =>
            {
                
                var singleBook = db.Books
                    .Include(b => b.Authors)
                    .Where(b => b.Id == bookId)
                    .Select(b => new
                    {
                        b.Id, 
                        b.Title,
                        b.Summary,
                        b.NumberOfPages,
                        publicationDate = b.PubDate.ToString("mm/dd/yyyy"),
                        b.ImageUrl,
                    })
                    .FirstOrDefault();

                if (singleBook == null)
                {
                    return Results.BadRequest();
                }
                return Results.Ok(singleBook);
            });
        }
    }
}
