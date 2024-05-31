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
        // get a user's shelves
        public static void Map(WebApplication app)
        {
            /* app.MapGet("/shelves/user/{userId}", (BooksDbContext db, int userId) => 
            {
                var userShelves = db.Shelves
                    .Include(s => s.BookShelves)
                    .Where(s => s.UserId == userId)
                    .Select(s => new
                    {
                        s.Id,
                        s.Name,
                        bookInformation = 
                    })
                    .ToList();

                if (userShelves == null)
                {
                    return Results.Empty;
                }
                return Results.Ok(userShelves);
            });
            */
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

            /* // get shelf details
            app.MapGet("/shelves/{shelfId}", (BooksDbContext db, int shelfId) =>
            {
                var singleShelf = db.Shelves
                .Include(s => s.Books)
                .Where(s => s.Id == shelfId)
                .FirstOrDefault();

                if (singleShelf == null)
                {
                    return Results.Empty;
                }
                return Results.Ok(singleShelf);
            });
            */
            // add book to shelf
            app.MapPost("/shelves/add-to-shelf", (BooksDbContext db, BookShelfDto dto) =>
            {
                var shelf = db.Shelves
                    .FirstOrDefault(s => s.Id == dto.ShelfId);
                if (shelf == null)
                {
                    return Results.BadRequest();
                }

                db.SaveChanges();
                return Results.Ok();    
            });

            // delete book from shelf 
            app.MapDelete("shelves/{shelfId}/delete-from-shelf/{bookId}", (BooksDbContext db, int shelfId, int bookId) =>
            {
                var shelf = db.Shelves
                    .FirstOrDefault(s => s.Id == shelfId);
                var deleteBook = db.Books.FirstOrDefault(b => b.Id == bookId);
                if (shelf == null || deleteBook == null)
                {
                    return Results.BadRequest();
                }
                db.SaveChanges();
                return Results.Ok();
            });
        }
    }
}
