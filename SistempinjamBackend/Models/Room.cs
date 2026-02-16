using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistempinjamBackend.Models;

[Table("rooms")]
public class Room
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required(ErrorMessage = "Nama ruangan wajib diisi.")]
    [MaxLength(100, ErrorMessage = "Nama ruangan maksimal 100 karakter.")]
    [Column("name")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Kode ruangan wajib diisi.")]
    [MaxLength(20, ErrorMessage = "Kode ruangan maksimal 20 karakter.")]
    [Column("room_code")]
    public string RoomCode { get; set; } = string.Empty;

    [Range(1, 1000, ErrorMessage = "Kapasitas harus antara 1 dan 1000.")]
    [Column("capacity")]
    public int Capacity { get; set; }

    [MaxLength(100, ErrorMessage = "Nama gedung maksimal 100 karakter.")]
    [Column("building")]
    public string Building { get; set; } = string.Empty;

    [MaxLength(20)]
    [Column("floor")]
    public string? Floor { get; set; }

    [Column("is_active")]
    public bool IsActive { get; set; } = true;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("deleted_at")]
    public DateTime? DeletedAt { get; set; }

    // Navigation: satu Room punya banyak Booking
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}