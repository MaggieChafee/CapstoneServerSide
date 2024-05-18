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
                    .OrderBy(b => b.PubDate)
                    .Take(25)
                    .ToList();
                if (recentReleases == null)
                {
                    return Results.BadRequest();
                }
                return Results.Ok(recentReleases);
            });
        }
    }
}
