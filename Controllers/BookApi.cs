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
                    .Where(b => b.Id == bookId)
                    .Include(b => b.Authors)
                    .Select(b => new
                    {
                        b.Id,
                        b.Title,
                        b.Summary,
                        b.NumberOfPages,
                        publicationDate = b.PubDate.ToString("MM/dd/yyyy"),
                        b.ImageUrl,
                        authorInformation = b.Authors
                            .Select(a => new { a.Id, a.FirstName, a.LastName })
                            .ToList(),
                    })
                    .FirstOrDefault();

                if (singleBook == null)
                {
                    return Results.BadRequest();
                }
                return Results.Ok(singleBook);
            });

            // get books by shelfId
            app.MapGet("/shelves/{shelfId}/books", (BooksDbContext db, int shelfId) =>
            {
                var books = db.BookShelves
                    .Where(x => x.ShelfId == shelfId)
                    .Select(b => b.Book)
                    .ToList();

                if (books == null)
                {
                    return Results.Empty;
                }

                return Results.Ok(books);
            });

            // get a book's average rating
            app.MapGet("/books/{bookId}/average-rating", (BooksDbContext db, int bookId) =>
            {

                var ratings = db.Reviews
                    .Where(r => r.BookId == bookId)
                    .Select(r => r.Rating)
                    .ToList();

                if (!ratings.Any())
                {
                    return Results.Ok(0);
                }

                var averageRating = ratings.Average();
                return Results.Ok(averageRating);
            });
        }
    }
}
