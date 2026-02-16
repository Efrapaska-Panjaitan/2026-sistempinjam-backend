using System.ComponentModel.DataAnnotations;

namespace SistempinjamBackend.DTOs;

// DTO untuk CREATE (POST /api/bookings)
public class BookingCreateDto
{
    [Required(ErrorMessage = "Nama peminjam wajib diisi.")]
    [MaxLength(100, ErrorMessage = "Nama peminjam maksimal 100 karakter.")]
    public string BorrowerName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Keperluan wajib diisi.")]
    [MaxLength(255, ErrorMessage = "Keperluan maksimal 255 karakter.")]
    public string PurposeOfUse { get; set; } = string.Empty;

    [Required(ErrorMessage = "Waktu mulai wajib diisi.")]
    public DateTime StartTime { get; set; }

    [Required(ErrorMessage = "Waktu selesai wajib diisi.")]
    public DateTime EndTime { get; set; }

    [MaxLength(500)]
    public string? Notes { get; set; }

    [Required(ErrorMessage = "Ruangan wajib dipilih.")]
    [Range(1, int.MaxValue, ErrorMessage = "RoomId tidak valid.")]
    public int RoomId { get; set; }
}

// DTO untuk UPDATE (PUT /api/bookings/{id})
public class BookingUpdateDto
{
    [Required(ErrorMessage = "Nama peminjam wajib diisi.")]
    [MaxLength(100)]
    public string BorrowerName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Keperluan wajib diisi.")]
    [MaxLength(255)]
    public string PurposeOfUse { get; set; } = string.Empty;

    [Required(ErrorMessage = "Waktu mulai wajib diisi.")]
    public DateTime StartTime { get; set; }

    [Required(ErrorMessage = "Waktu selesai wajib diisi.")]
    public DateTime EndTime { get; set; }

    [MaxLength(500)]
    public string? Notes { get; set; }

    [Required(ErrorMessage = "Ruangan wajib dipilih.")]
    [Range(1, int.MaxValue)]
    public int RoomId { get; set; }
}

// DTO untuk UPDATE STATUS saja (PATCH /api/bookings/{id}/status)
public class BookingStatusDto
{
    [Required(ErrorMessage = "Status wajib diisi.")]
    [RegularExpression("^(Pending|Approved|Rejected)$",
        ErrorMessage = "Status harus Pending, Approved, atau Rejected.")]
    public string Status { get; set; } = string.Empty;
}

// DTO untuk RESPONSE (data yang dikirim ke client)
public class BookingResponseDto
{
    public int Id { get; set; }
    public string BorrowerName { get; set; } = string.Empty;
    public string PurposeOfUse { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public int RoomId { get; set; }
    public string RoomName { get; set; } = string.Empty;
    public string RoomCode { get; set; } = string.Empty;
    public string Building { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
