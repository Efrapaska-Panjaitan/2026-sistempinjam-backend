using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistempinjamBackend.Models;

[Table("bookings")]
public class Booking
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required(ErrorMessage = "Nama peminjam wajib diisi.")]
    [MaxLength(100, ErrorMessage = "Nama peminjam maksimal 100 karakter.")]
    [Column("borrower_name")]
    public string BorrowerName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Keperluan wajib diisi.")]
    [MaxLength(255, ErrorMessage = "Keperluan maksimal 255 karakter.")]
    [Column("purpose_of_use")]
    public string PurposeOfUse { get; set; } = string.Empty;

    [Required(ErrorMessage = "Waktu mulai wajib diisi.")]
    [Column("start_time")]
    public DateTime StartTime { get; set; }

    [Required(ErrorMessage = "Waktu selesai wajib diisi.")]
    [Column("end_time")]
    public DateTime EndTime { get; set; }

    // Status: Pending | Approved | Rejected
    [MaxLength(20)]
    [Column("status")]
    public string Status { get; set; } = "Pending";

    [MaxLength(500)]
    [Column("notes")]
    public string? Notes { get; set; }

    // Foreign key ke Room
    [Required(ErrorMessage = "Ruangan wajib dipilih.")]
    [Column("room_id")]
    public int RoomId { get; set; }

    // Navigation property
    [ForeignKey("RoomId")]
    public Room? Room { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Soft delete: DeletedAt berisi tanggal jika dihapus, null jika masih aktif
    [Column("deleted_at")]
    public DateTime? DeletedAt { get; set; }
}