using BillboardServer.Models;
using Microsoft.EntityFrameworkCore;

namespace BillboardServer.Data;

public class BillboardDbContext : DbContext
{
    public DbSet<Message> Messages { get; set; }

    public BillboardDbContext(DbContextOptions<BillboardDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Message>()
            .HasKey(m => m.Id);

        modelBuilder.Entity<Message>()
            .Property(m => m.Content)
            .IsRequired();

        modelBuilder.Entity<Message>()
            .Property(m => m.CreatedAt)
            .IsRequired();
    }
}
