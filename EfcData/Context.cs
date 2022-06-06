using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace EfcData;

public class Context : DbContext
{
    public DbSet<Book> Book { get; set; } // this represents the Book table in the database
    public DbSet<Author> Author { get; set; } // this represents the Author table in the database
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source = ../EfcData/exam_db.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // modelBuilder.Entity<Book>(entity =>
        // {
        //     entity.HasOne(book => book.Author)
        //         .WithMany(author => author.Books);
        // });
        // modelBuilder.Entity<Author>(entity =>
        // {
        //     entity.HasMany<Book>(a => a.Books)
        //         .WithOne(b => b.Author);
        // });
    }
}