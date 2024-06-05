using System.Reflection.PortableExecutable;
using Books.Controllers;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Books.Models;
using Books.DTOs;

namespace Books.Controllers
{
    public class UserApi
    {
        public static void Map(WebApplication app)
        {
            app.MapGet("/users/{userId}", (BooksDbContext db, int userId) =>
            {
                var singleUser = db.Users.FirstOrDefault(u => u.Id == userId);

                if (singleUser == null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(singleUser);
            });

            // check to see if a user is in the system based on uid
            app.MapGet("/checkuser/{uid}", (BooksDbContext db, string uid) =>
            {
                var user = db.Users.FirstOrDefault(u => u.Uid == uid);

                if (user == null)
                {
                    return Results.NotFound();
                }
                return Results.Ok(user);
            });

            // create user
            app.MapPost("/users", (BooksDbContext db, User newUser) =>
            {
                db.Users.Add(newUser);
                db.SaveChanges();

                return Results.Created($"/users/{newUser.Id}", newUser);
            });

            // create user and shelves
            app.MapPost("/users-and-shelves", (BooksDbContext db, User newUser) =>
            {
                db.Users.Add(newUser);
                db.SaveChanges();
                Shelf favorites = new Shelf()
                {
                    Name = "Favorites",
                    UserId = newUser.Id,
                };
                Shelf currentlyReading = new Shelf()
                {
                    Name = "Currently Reading",
                    UserId = newUser.Id,
                };
                Shelf wantToRead = new Shelf()
                {
                    Name = "Want To Read",
                    UserId = newUser.Id,
                };
                Shelf read = new Shelf()
                {
                    Name = "Read",
                    UserId = newUser.Id,
                };
                db.Shelves.Add(favorites);
                db.Shelves.Add(currentlyReading);
                db.Shelves.Add(wantToRead);
                db.Shelves.Add(read);
                db.SaveChanges();

                return Results.Created($"/users/{newUser.Id}", newUser);
            });
        }
    }
}
