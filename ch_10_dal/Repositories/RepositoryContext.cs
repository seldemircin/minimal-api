using Microsoft.EntityFrameworkCore;

namespace ch_10_dal.Repositories;

public class RepositoryContext : DbContext
{
    public RepositoryContext(DbContextOptions  options) : base(options)
    {
        
    }
    
    public DbSet<Book> Books { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // seed data
        
        modelBuilder.Entity<Book>().HasData(
            new Book()
            {
                Id = 1,Title = "Book 1", Price = 10
            },
            new Book()
            {
                Id = 2,Title = "Book 2", Price = 20
            },
            new Book()
            {
                Id = 3,Title = "Book 3", Price = 30
            }
        );
    }
}