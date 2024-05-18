using Microsoft.EntityFrameworkCore;
using Books.Models;

namespace Books
{
    public class BooksDbContext : DbContext
    {
        public DbSet<Book> Books { get; set;}
        public DbSet<User> Users { get; set;}
        public DbSet<Shelf> Shelves { get; set;}
        public DbSet<Review> Reviews { get; set;}

        public BooksDbContext(DbContextOptions<BooksDbContext> context) : base(context) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(new User[]
            {
                new User { Id = 1, Username = "DameMaggieSmith", Uid = "1234", Email = "myemail@me.com", ImageUrl = "https://m.media-amazon.com/images/M/MV5BY2U0ZWE1MDItZDIxNS00ZDMzLTliYWUtNmE3NTAxNjQ1MjQ2XkEyXkFqcGdeQXVyMzQ3Nzk5MTU@._V1_.jpg" },
            });

            modelBuilder.Entity<Book>().HasData(new Book[]
            {
                new Book { Id = 1, Title = "A Room with a View", Summary = "Lucy is a well-mannered Edwardian lady who finds that true love has no interest in playing by her rules. But how can she choose between what she wants and what everyone around her expects her to want? This gentle but sharp comedy has it all: surprise encounters, jealousy and revenge, conventional fools and unconventional sages, confrontation, loss, and eventual triumph.", NumberOfPages = 172, PubDate = new DateTime(2011, 11, 1), ImageUrl = "https://upload.wikimedia.org/wikipedia/en/8/8e/A_Room_with_a_View.jpg"  }
            });

            modelBuilder.Entity<Author>().HasData(new Author[]
            {
                new Author { Id = 1, FirstName = "E.M.", LastName = "Forster", Biography = "TBD", ImageUrl = "https://cdn.britannica.com/82/11782-004-305E324D/EM-Forster.jpg"}
            });
        }
    }
}
