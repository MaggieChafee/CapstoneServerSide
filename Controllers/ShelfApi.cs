using System.Reflection.PortableExecutable;
using Books.Controllers;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;

namespace Books.Controllers
{
    public class ShelfApi
    {
        // get a user's shelves
        public static void Map(WebApplication app)
        {
            app.MapGet("/shelves/user/{userId}", (BooksDbContext db, int userId) => 
            {
                var userShelves = db.Shelves
                    .Include(s => s.Books)
                    .Where(s => s.UserId == userId)
                    .ToList();

                if (userShelves == null)
                {
                    return Results.Empty;
                }
                return Results.Ok(userShelves);
            });
        }
    }
}
