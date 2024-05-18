using System.Reflection.PortableExecutable;
using Books.Controllers;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;

namespace Books.Controllers
{
    public class UserApi
    {
        public static void Map(WebApplication app)
        {
            app.MapGet("", () => {

            });
        }
    }
}
