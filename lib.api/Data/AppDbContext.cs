using lib.api.Models;
using Microsoft.EntityFrameworkCore;

namespace lib.api.Data;

public class AppDbContext(
    DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<Borrowing> Borrowings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.HasIndex(u => u.Email).IsUnique();
            entity.Property(u => u.Email).IsRequired();
            entity.HasData( 
                new User { Id = 1, Email = "janko@gmail.com" },
                new User { Id = 2, Email = "ferko@gmail.com" }
                );
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(b => b.Id);
            entity.Property(b => b.Title).IsRequired();
            entity.Property(b => b.Author).IsRequired();
            entity.Property(b => b.Status).IsRequired().HasConversion<string>();
            entity.HasData(
                new Book(title: "Book 1", author: "Author 1") { Id = 1 },
                new Book(title: "Book 2", author: "Author 2") { Id = 2 }
                );
        });

        modelBuilder.Entity<Borrowing>(entity =>
        {
            entity.HasKey(b => b.Id);
            entity.Property(b => b.DateCreated).IsRequired();
            entity.Property(b => b.DateDue).IsRequired();
            entity.Property(b => b.BorrowStatus).IsRequired().HasConversion<string>();

            entity.HasOne(b => b.User)
                .WithMany(u => u.Borrowings)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(b => b.Book)
                .WithMany(b => b.Borrowings)
                .HasForeignKey(b => b.BookId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}