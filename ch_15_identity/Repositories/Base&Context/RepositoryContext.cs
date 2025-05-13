using ch_15_identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ch_15_identity.Repositories.Base_Context;

public class RepositoryContext : IdentityDbContext<User>
{
    public RepositoryContext(DbContextOptions options) : base(options)
    {
        
    }
    
    
    public DbSet<Book> Books { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Category>().HasData(
            
            new Category { Id = 1, Name = "Roman" },
            new Category { Id = 2, Name = "Bilim" },
            new Category { Id = 3, Name = "Tarih" },
            new Category { Id = 4, Name = "Felsefe" },
            new Category { Id = 5, Name = "Teknoloji" }
        );

        modelBuilder.Entity<Book>().HasData(
            new Book { Id = 1, Title = "Suç ve Ceza", Price = 89.99m, CategoryId = 1 },
            new Book { Id = 2, Title = "Sefiller", Price = 79.50m, CategoryId = 1 },

            new Book { Id = 3, Title = "Zamanın Kısa Tarihi", Price = 105.00m, CategoryId = 2 },
            new Book { Id = 4, Title = "Kozmos", Price = 92.25m, CategoryId = 2 },

            new Book { Id = 5, Title = "Nutuk", Price = 65.00m, CategoryId = 3 },
            new Book { Id = 6, Title = "Medeniyetler Çatışması", Price = 70.00m, CategoryId = 3 },

            new Book { Id = 7, Title = "Devlet", Price = 50.00m, CategoryId = 4 },
            new Book { Id = 8, Title = "Varoluşçuluk", Price = 60.00m, CategoryId = 4 },

            new Book { Id = 9, Title = "Yapay Zeka 101", Price = 110.00m, CategoryId = 5 },
            new Book { Id = 10, Title = "Kodlama Mantığı", Price = 95.00m, CategoryId = 5 }
        );

        modelBuilder.Entity<IdentityRole>().HasData(
            new IdentityRole
            {
                Name = "Admin",
                NormalizedName = "ADMIN"
            },
            new IdentityRole()
            {
                Name = "User",
                NormalizedName = "USER"
            }
        );
    }

}