using System.ComponentModel.DataAnnotations;

namespace SistempinjamBackend.DTOs;

public class RoomCreateDto
{
    [Required(ErrorMessage = "Nama ruangan wajib diisi.")]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Kode ruangan wajib diisi.")]
    [MaxLength(20)]
    public string RoomCode { get; set; } = string.Empty;

    [Range(1, 1000, ErrorMessage = "Kapasitas harus antara 1 dan 1000.")]
    public int Capacity { get; set; }

    [MaxLength(100)]
    public string Building { get; set; } = string.Empty;

    [MaxLength(20)]
    public string? Floor { get; set; }
}

public class RoomUpdateDto
{
    [Required(ErrorMessage = "Nama ruangan wajib diisi.")]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Kode ruangan wajib diisi.")]
    [MaxLength(20)]
    public string RoomCode { get; set; } = string.Empty;

    [Range(1, 1000)]
    public int Capacity { get; set; }

    [MaxLength(100)]
    public string Building { get; set; } = string.Empty;

    [MaxLength(20)]
    public string? Floor { get; set; }

    public bool IsActive { get; set; } = true;
}

public class RoomResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string RoomCode { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public string Building { get; set; } = string.Empty;
    public string? Floor { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}