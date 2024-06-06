using System.Reflection.PortableExecutable;
using Books.Controllers;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Books.Models;
using Books.DTOs;
using System.Xml;

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
                        reviewDate = r.DateCreated.ToString("M/d/yyyy"),
                        r.Comment,
                        userName = r.User.Username,
                        r.BookId,
                        r.UserId
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
                        reviewDate = r.DateCreated.ToString("M/d/yyyy"),
                        r.Comment,
                        userName = r.User.Username,
                        r.BookId,
                        r.UserId
                    })
                    .FirstOrDefault();

                if (singleReview == null)
                {
                    return Results.Empty;
                }
                return Results.Ok(singleReview);
            });

            // get user's reviews
            app.MapGet("reviews/user/{userId}", (BooksDbContext db, int userId) =>
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

            // create review
            app.MapPost("/reviews", (BooksDbContext db, CreateReviewDto dto) =>
            {

                var newReview = new Review()
                {
                    Rating = dto.Rating,
                    Comment = dto.Comment,
                    DateCreated = dto.DateCreated,
                    UserId = dto.UserId,
                    User = db.Users.FirstOrDefault(u => u.Id == dto.UserId),
                    BookId = dto.BookId,
                    Book = db.Books.FirstOrDefault(b => b.Id == dto.BookId),
                };
                db.Reviews.Add(newReview);
                db.SaveChanges();

                return Results.Created($"/reviews/{newReview.Id}", newReview);
            });
            // update review
            app.MapPut("/reviews/{reviewId}", (BooksDbContext db, int reviewId, UpdateReviewDto dto) =>
            {
                var reviewToUpdate = db.Reviews.FirstOrDefault(r => r.Id == reviewId);
                if (reviewToUpdate == null)
                {
                    return Results.BadRequest();
                }

                reviewToUpdate.Comment = dto.Comment;
                reviewToUpdate.DateCreated = dto.DateCreated;
                reviewToUpdate.Rating = dto.Rating;

                db.SaveChanges();
                return Results.Ok("Successfully updated!");
            });

            // delete review
            app.MapDelete("/reviews/{reviewId}", (BooksDbContext db, int reviewId) =>
            {
                var reviewToDelete = db.Reviews.FirstOrDefault(r => r.Id == reviewId);
                if (reviewToDelete == null)
                {
                    return Results.BadRequest();
                }

                db.Reviews.Remove(reviewToDelete);
                db.SaveChanges();

                return Results.Ok();
            });
        }
    }
}
