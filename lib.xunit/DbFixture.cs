using lib.api.Data;
using lib.api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace lib.xunit;

// ReSharper disable once ConvertToUsingDeclaration

public class DbFixture : IDisposable
{
    public DbContextOptions<AppDbContext> Options { get; }

    public DbFixture()
    {
        Options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase($"db{Guid.NewGuid()}")
            .ConfigureWarnings(d => d.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;
        
        using (var dbContext = new AppDbContext(Options))
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
            
            dbContext.AddRange(
                new User { Id = 11, Email = "testuser1@gmail.com" },
                new User { Id = 22, Email = "testuser2@gmail.com" },
                new Book { Id = 11, Title = "Test Book 1", Author = "Test Author 1"},
                new Book { Id = 22, Title = "Test Book 2", Author = "Test Author 2"});

            dbContext.SaveChanges();
        }
    }

    public void Dispose()
    {
        using (var dbContext = new AppDbContext(Options))
        {
            dbContext.Database.EnsureDeleted();
        }
        GC.SuppressFinalize(this);
    }
}