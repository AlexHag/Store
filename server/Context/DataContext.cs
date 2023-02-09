using Microsoft.EntityFrameworkCore;
using server.Models;

namespace server.Context;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    { }

    public DbSet<User> Users { get; set; }
    public DbSet<Store> Stores { get; set; }
    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(p => p.Email)
            .IsUnique();
        
        modelBuilder.Entity<Store>()
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(p => p.StoreOwnerId)
            .OnDelete(DeleteBehavior.NoAction);
        
        modelBuilder.Entity<Product>()
            .HasOne<Store>()
            .WithMany()
            .HasForeignKey(p => p.StoreId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}