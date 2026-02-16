using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistempinjamBackend.Data;
using SistempinjamBackend.DTOs;
using SistempinjamBackend.Models;

namespace SistempinjamBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class BookingsController : ControllerBase
{
    private readonly AppDbContext _context;

    public BookingsController(AppDbContext context)
    {
        _context = context;
    }

    // ─────────────────────────────────────────────
    // GET /api/bookings
    // Query params: search, status, page, pageSize
    // ─────────────────────────────────────────────
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? search,
        [FromQuery] string? status,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var query = _context.Bookings
            .Include(b => b.Room)
            .AsQueryable();

        // Filter pencarian berdasarkan nama peminjam atau keperluan
        if (!string.IsNullOrWhiteSpace(search))
        {
            search = search.ToLower();
            query = query.Where(b =>
                b.BorrowerName.ToLower().Contains(search) ||
                b.PurposeOfUse.ToLower().Contains(search));
        }

        // Filter berdasarkan status
        if (!string.IsNullOrWhiteSpace(status))
        {
            query = query.Where(b => b.Status == status);
        }

        var total = await query.CountAsync();

        var bookings = await query
            .OrderByDescending(b => b.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(b => new BookingResponseDto
            {
                Id            = b.Id,
                BorrowerName  = b.BorrowerName,
                PurposeOfUse  = b.PurposeOfUse,
                StartTime     = b.StartTime,
                EndTime       = b.EndTime,
                Status        = b.Status,
                Notes         = b.Notes,
                RoomId        = b.RoomId,
                RoomName      = b.Room != null ? b.Room.Name     : string.Empty,
                RoomCode      = b.Room != null ? b.Room.RoomCode : string.Empty,
                Building      = b.Room != null ? b.Room.Building : string.Empty,
                CreatedAt     = b.CreatedAt,
                UpdatedAt     = b.UpdatedAt,
            })
            .ToListAsync();

        return Ok(new
        {
            data       = bookings,
            total,
            page,
            pageSize,
            totalPages = (int)Math.Ceiling((double)total / pageSize)
        });
    }

    // ─────────────────────────────────────────────
    // GET /api/bookings/{id}
    // ─────────────────────────────────────────────
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var booking = await _context.Bookings
            .Include(b => b.Room)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (booking == null)
            return NotFound(new { message = $"Booking dengan id {id} tidak ditemukan." });

        var response = new BookingResponseDto
        {
            Id           = booking.Id,
            BorrowerName = booking.BorrowerName,
            PurposeOfUse = booking.PurposeOfUse,
            StartTime    = booking.StartTime,
            EndTime      = booking.EndTime,
            Status       = booking.Status,
            Notes        = booking.Notes,
            RoomId       = booking.RoomId,
            RoomName     = booking.Room?.Name     ?? string.Empty,
            RoomCode     = booking.Room?.RoomCode ?? string.Empty,
            Building     = booking.Room?.Building ?? string.Empty,
            CreatedAt    = booking.CreatedAt,
            UpdatedAt    = booking.UpdatedAt,
        };

        return Ok(response);
    }

    // ─────────────────────────────────────────────
    // POST /api/bookings
    // ─────────────────────────────────────────────
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] BookingCreateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Validasi: EndTime harus setelah StartTime
        if (dto.EndTime <= dto.StartTime)
            return BadRequest(new { message = "Waktu selesai harus setelah waktu mulai." });

        // Validasi: room harus ada dan aktif
        var room = await _context.Rooms.FindAsync(dto.RoomId);
        if (room == null)
            return BadRequest(new { message = "Ruangan tidak ditemukan." });
        if (!room.IsActive)
            return BadRequest(new { message = "Ruangan tidak aktif dan tidak bisa dipesan." });

        // Validasi: cek konflik waktu (room sudah dipesan di slot yang sama)
        var conflict = await _context.Bookings.AnyAsync(b =>
            b.RoomId == dto.RoomId &&
            b.Status != "Rejected" &&
            b.StartTime < dto.EndTime &&
            b.EndTime   > dto.StartTime);

        if (conflict)
            return Conflict(new { message = "Ruangan sudah dipesan pada waktu yang sama." });

        var booking = new Booking
        {
            BorrowerName = dto.BorrowerName,
            PurposeOfUse = dto.PurposeOfUse,
            StartTime    = dto.StartTime,
            EndTime      = dto.EndTime,
            Notes        = dto.Notes,
            RoomId       = dto.RoomId,
            Status       = "Pending",
            CreatedAt    = DateTime.UtcNow,
            UpdatedAt    = DateTime.UtcNow,
        };

        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = booking.Id }, new { id = booking.Id, message = "Booking berhasil dibuat." });
    }

    // ─────────────────────────────────────────────
    // PUT /api/bookings/{id}
    // ─────────────────────────────────────────────
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] BookingUpdateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var booking = await _context.Bookings.FindAsync(id);
        if (booking == null)
            return NotFound(new { message = $"Booking dengan id {id} tidak ditemukan." });

        // Booking yang sudah Approved/Rejected tidak bisa diedit
        if (booking.Status != "Pending")
            return BadRequest(new { message = "Hanya booking berstatus Pending yang bisa diedit." });

        if (dto.EndTime <= dto.StartTime)
            return BadRequest(new { message = "Waktu selesai harus setelah waktu mulai." });

        var room = await _context.Rooms.FindAsync(dto.RoomId);
        if (room == null)
            return BadRequest(new { message = "Ruangan tidak ditemukan." });

        // Cek konflik (kecuali booking ini sendiri)
        var conflict = await _context.Bookings.AnyAsync(b =>
            b.Id     != id &&
            b.RoomId == dto.RoomId &&
            b.Status != "Rejected" &&
            b.StartTime < dto.EndTime &&
            b.EndTime   > dto.StartTime);

        if (conflict)
            return Conflict(new { message = "Ruangan sudah dipesan pada waktu yang sama." });

        booking.BorrowerName = dto.BorrowerName;
        booking.PurposeOfUse = dto.PurposeOfUse;
        booking.StartTime    = dto.StartTime;
        booking.EndTime      = dto.EndTime;
        booking.Notes        = dto.Notes;
        booking.RoomId       = dto.RoomId;
        booking.UpdatedAt    = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    // ─────────────────────────────────────────────
    // PATCH /api/bookings/{id}/status
    // Khusus update status: Pending -> Approved/Rejected
    // ─────────────────────────────────────────────
    [HttpPatch("{id:int}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] BookingStatusDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var booking = await _context.Bookings.FindAsync(id);
        if (booking == null)
            return NotFound(new { message = $"Booking dengan id {id} tidak ditemukan." });

        booking.Status    = dto.Status;
        booking.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return Ok(new { message = $"Status booking diubah menjadi {dto.Status}." });
    }

    // ─────────────────────────────────────────────
    // DELETE /api/bookings/{id}   →  Soft Delete
    // ─────────────────────────────────────────────
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var booking = await _context.Bookings.FindAsync(id);
        if (booking == null)
            return NotFound(new { message = $"Booking dengan id {id} tidak ditemukan." });

        // Soft delete: set DeletedAt, data tetap ada di database
        booking.DeletedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return Ok(new { message = "Booking berhasil dihapus." });
    }
}
