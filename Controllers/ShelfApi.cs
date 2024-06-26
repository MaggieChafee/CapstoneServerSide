﻿using System.Reflection.PortableExecutable;
using Books.Controllers;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Books.Models;
using Books.DTOs;

namespace Books.Controllers
{
    public class ShelfApi
    {

        public static void Map(WebApplication app)
        {
            // get a user's shelves
            app.MapGet("/shelves/user/{userId}", (BooksDbContext db, int userId) =>
            {
                var userShelves = db.Shelves
                    .Include(s => s.BookShelves)
                    .ThenInclude(s => s.Book)
                    .Where(s => s.UserId == userId)
                    .Select(s => new
                    {
                        s.Id,
                        s.Name,
                        bookInformation = s.BookShelves.Select(s => new
                        {
                            s.Id,
                            s.BookId,
                            bookTitle = s.Book.Title,
                            bookImage = s.Book.ImageUrl
                        }).Take(4)
                    })
                    .ToList();

                if (userShelves == null)
                {
                    return Results.Empty;
                }
                return Results.Ok(userShelves);
            });

            // create a new shelf 
            app.MapPost("/shelves", (BooksDbContext db, CreateShelfDto dto) =>
            {
                var newShelf = new Shelf()
                {
                    UserId = dto.UserId,
                    Name = dto.Name,
                };
                db.Shelves.Add(newShelf);
                db.SaveChanges();

                return Results.Created($"/shelves/{newShelf.Id}", newShelf);
            });

            // get shelf details
            app.MapGet("/shelves/{shelfId}", (BooksDbContext db, int shelfId) =>
            {
                var singleShelf = db.Shelves
                .Include(s => s.BookShelves)
                .ThenInclude(s => s.Book)
                .Where(s => s.Id == shelfId)
                .FirstOrDefault();

                if (singleShelf == null)
                {
                    return Results.Empty;
                }
                return Results.Ok(singleShelf);
            });

            // add book to shelf
            app.MapPost("/bookshelves", (BooksDbContext db, BookShelfDto dto) =>
            {
                var newBookShelf = new BookShelf()
                {
                    BookId = dto.BookId,
                    Book = db.Books.FirstOrDefault(b => b.Id == dto.BookId),
                    ShelfId = dto.ShelfId,
                    Shelf = db.Shelves.FirstOrDefault(s => s.Id == dto.ShelfId)
                };

                if (newBookShelf == null)
                {
                    return Results.BadRequest();
                }
                db.BookShelves.Add(newBookShelf);
                db.SaveChanges();
                return Results.Created();
            });

            // delete book from shelf 
            app.MapDelete("bookshelves/{bookShelfId}", (BooksDbContext db, int bookShelfId) =>
            {
                var bookShelfToDelete = db.BookShelves.FirstOrDefault(bs => bs.Id == bookShelfId);

                if (bookShelfToDelete == null)
                {
                    return Results.BadRequest();
                }
                db.BookShelves.Remove(bookShelfToDelete);
                db.SaveChanges();
                return Results.Ok();
            });

            // update shelf book is on
            app.MapPut("/bookshelves/{bookShelfId}", (BooksDbContext db, int bookShelfId, BookShelfDto dto) =>
            {
                var bookShelfToUpdate = db.BookShelves.FirstOrDefault(bs => bs.Id == bookShelfId);
                if (bookShelfToUpdate == null)
                {
                    return Results.BadRequest();
                }
                bookShelfToUpdate.ShelfId = dto.ShelfId;
                bookShelfToUpdate.Shelf = db.Shelves.FirstOrDefault(s => s.Id == dto.ShelfId);
                db.SaveChanges();
                return Results.Ok("Book successfully moved to new shelf");
            });

            // get shelf book is on
            app.MapGet("/books/{bookId}/shelves/{userId}", (BooksDbContext db, int bookId, int userId) =>
            {
                var shelfCheck = db.Shelves
                    .Include(s => s.BookShelves)
                    .Where(s => s.UserId == userId)
                    .Where(b => b.BookShelves.Any(b => b.BookId == bookId))
                    .FirstOrDefault();

                if (shelfCheck == null)
                {
                    return Results.Json(new { });
                }

                return Results.Ok(shelfCheck);

            });

            // get bookshelf based on bookId and userId
            app.MapGet("/books/{bookId}/bookshelves/{shelfId}", (BooksDbContext db, int bookId, int shelfId) =>
            {
                var bookShelf = db.BookShelves.FirstOrDefault(b => b.BookId == bookId && b.ShelfId == shelfId);
                if (bookShelf == null)
                {
                    return Results.Json(new { });
                }

                return Results.Ok(bookShelf);
            });
        }
    }
}