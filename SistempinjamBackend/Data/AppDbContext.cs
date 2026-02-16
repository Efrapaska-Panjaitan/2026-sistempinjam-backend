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
    }
}