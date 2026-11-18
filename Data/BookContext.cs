using Books.Models;
using Microsoft.EntityFrameworkCore;

namespace Books.Data;

public class BookContext(DbContextOptions<BookContext> options) : DbContext(options)
{
    public DbSet<Book> Books => Set<Book>();
    public DbSet<Comment> Comments => Set<Comment>();
    public DbSet<BookImage> BookImages => Set<BookImage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>().Property(book => book.Price).HasPrecision(10, 2);
        modelBuilder.Entity<Comment>()
            .HasOne(comment => comment.Book)
            .WithMany(book => book.Comments)
            .HasForeignKey(comment => comment.BookId);
        modelBuilder.Entity<BookImage>()
            .HasOne(image => image.Book)
            .WithMany(book => book.Images)
            .HasForeignKey(image => image.BookId);
    }
}
