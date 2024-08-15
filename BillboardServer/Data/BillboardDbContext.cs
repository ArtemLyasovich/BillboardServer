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
        modelBuilder.Entity<Message>(entity =>
        {
            entity.ToTable("messages");

            entity.HasKey(m => m.Id);

            entity.Property(m => m.Id)
                .HasColumnName("id");

            entity.Property(m => m.Content)
                .IsRequired()
                .HasColumnName("content");

            entity.Property(m => m.CreatedAt)
                .IsRequired()
                .HasColumnName("createdat");
        });
    }
}
