using System.Reflection.PortableExecutable;
using Books.Controllers;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Books.Models;

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
                    return Results.BadRequest();
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
        }
    }
}
