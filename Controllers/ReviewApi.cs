using System.Reflection.PortableExecutable;
using Books.Controllers;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;

namespace Books.Controllers
{
    public class ReviewApi
    {
        public static void Map(WebApplication app)
        {
            // get reviews for a single book
            app.MapGet("/books/{bookId}/reviews", (BooksDbContext db, int bookId) => 
            {
                var bookReviews = db.Reviews
                    .Where(r => r.BookId == bookId)
                    .Select(r => new
                    {
                        r.Id,
                        r.Rating,
                        reviewDate = r.DateCreated.ToString("mm/dd/yyyy"),
                        r.Comment,
                        userName = r.User.Username,
                    })
                    .ToList();

                if (bookReviews == null)
                {
                    return Results.Empty;
                }
                return Results.Ok(bookReviews);
            });

            // if the user has reviewed a book, get that review
            app.MapGet("/books/{bookId}/userReview/{userId}", (BooksDbContext db, int bookId, int userId) =>
            {
                var singleReview = db.Reviews
                    .Where(r => r.BookId == bookId && r.UserId == userId)
                    .Select(r => new
                    {
                        r.Id,
                        r.Rating,
                        reviewDate = r.DateCreated.ToString("mm/dd/yyyy"),
                        r.Comment,
                        userName = r.User.Username,
                    })
                    .FirstOrDefault();

                if (singleReview == null)
                {
                    return Results.Empty;
                }
                return Results.Ok(singleReview);
            });

            // get user's reviews
            app.MapGet("/reviews/user/{userId}", (BooksDbContext db, int userId) =>
            {
                var usersReviews = db.Reviews
                    .Where(r => r.UserId == userId)
                    .Select( r => new
                    {
                        r.Id,
                        r.UserId,
                        r.BookId,
                        r.DateCreated,
                        r.Rating,
                        userName = r.User.Username,
                        r.Comment,
                        bookTitle = r.Book.Title,
                    })
                    .OrderByDescending(r => r.DateCreated)
                    .ToList();

                if (usersReviews == null)
                {
                    return Results.Empty;
                }
                return Results.Ok(usersReviews);
            });
        }
    }
}
