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
            dbContext.Database.EnsureCreated();
            dbContext.AddRange(
                new User { Id = Ids.Id11, Email = "testuser1@gmail.com" },
                new User { Id = Ids.Id22, Email = "testuser2@gmail.com" },
                new Book("Test Book 1", "Test Author 1") { Id = Ids.Id11 },
                new Book("Test Book 1", "Test Author 1") { Id = Ids.Id22 });

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