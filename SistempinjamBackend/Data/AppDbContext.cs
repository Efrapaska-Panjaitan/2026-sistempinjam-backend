using Microsoft.EntityFrameworkCore;
using SistempinjamBackend.Models;

namespace SistempinjamBackend.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Room> Rooms { get; set; }
    public DbSet<Booking> Bookings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // === ROOM ===
        modelBuilder.Entity<Room>(entity =>
        {
            // Global query filter: hanya tampilkan room yang belum dihapus (soft delete)
            entity.HasQueryFilter(r => r.DeletedAt == null);

            entity.HasIndex(r => r.RoomCode).IsUnique();
        });

        // === BOOKING ===
        modelBuilder.Entity<Booking>(entity =>
        {
            // Global query filter: hanya tampilkan booking yang belum dihapus
            entity.HasQueryFilter(b => b.DeletedAt == null);

            // Relasi: banyak Booking -> satu Room
            entity.HasOne(b => b.Room)
                .WithMany(r => r.Bookings)
                .HasForeignKey(b => b.RoomId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}