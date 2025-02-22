using BookManagement.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookManagement.Data.Persistence;

public class BookManagementDbContext(DbContextOptions<BookManagementDbContext> options) : IdentityDbContext<User>(options)
{
    public DbSet<Book> Books { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>()
            .HasIndex(b => b.Title)
            .IsUnique();

        base.OnModelCreating(modelBuilder);
    }
}