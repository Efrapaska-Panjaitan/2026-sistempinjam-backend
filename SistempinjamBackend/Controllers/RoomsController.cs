using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistempinjamBackend.Data;
using SistempinjamBackend.DTOs;
using SistempinjamBackend.Models;

namespace SistempinjamBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class RoomsController : ControllerBase
{
    private readonly AppDbContext _context;

    public RoomsController(AppDbContext context)
    {
        _context = context;
    }

    // ─────────────────────────────────────────────
    // GET /api/rooms
    // ─────────────────────────────────────────────
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? search)
    {
        var query = _context.Rooms.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            search = search.ToLower();
            query = query.Where(r =>
                r.Name.ToLower().Contains(search) ||
                r.RoomCode.ToLower().Contains(search) ||
                r.Building.ToLower().Contains(search));
        }

        var rooms = await query
            .OrderBy(r => r.Building).ThenBy(r => r.Name)
            .Select(r => new RoomResponseDto
            {
                Id        = r.Id,
                Name      = r.Name,
                RoomCode  = r.RoomCode,
                Capacity  = r.Capacity,
                Building  = r.Building,
                Floor     = r.Floor,
                IsActive  = r.IsActive,
                CreatedAt = r.CreatedAt,
            })
            .ToListAsync();

        return Ok(rooms);
    }

    // ─────────────────────────────────────────────
    // GET /api/rooms/{id}
    // ─────────────────────────────────────────────
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var room = await _context.Rooms
            .FirstOrDefaultAsync(r => r.Id == id);

        if (room == null)
            return NotFound(new { message = $"Ruangan dengan id {id} tidak ditemukan." });

        var response = new RoomResponseDto
        {
            Id        = room.Id,
            Name      = room.Name,
            RoomCode  = room.RoomCode,
            Capacity  = room.Capacity,
            Building  = room.Building,
            Floor     = room.Floor,
            IsActive  = room.IsActive,
            CreatedAt = room.CreatedAt,
        };

        return Ok(response);
    }

    // ─────────────────────────────────────────────
    // POST /api/rooms
    // ─────────────────────────────────────────────
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] RoomCreateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Cek duplikat kode ruangan
        var exists = await _context.Rooms
            .AnyAsync(r => r.RoomCode == dto.RoomCode);
        if (exists)
            return Conflict(new { message = $"Kode ruangan '{dto.RoomCode}' sudah digunakan." });

        var room = new Room
        {
            Name      = dto.Name,
            RoomCode  = dto.RoomCode,
            Capacity  = dto.Capacity,
            Building  = dto.Building,
            Floor     = dto.Floor,
            CreatedAt = DateTime.UtcNow,
        };

        _context.Rooms.Add(room);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = room.Id }, new { id = room.Id, message = "Ruangan berhasil dibuat." });
    }

    // ─────────────────────────────────────────────
    // PUT /api/rooms/{id}
    // ─────────────────────────────────────────────
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] RoomUpdateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var room = await _context.Rooms.FindAsync(id);
        if (room == null)
            return NotFound(new { message = $"Ruangan dengan id {id} tidak ditemukan." });

        // Cek duplikat kode (selain room ini sendiri)
        var codeExists = await _context.Rooms
            .AnyAsync(r => r.RoomCode == dto.RoomCode && r.Id != id);
        if (codeExists)
            return Conflict(new { message = $"Kode ruangan '{dto.RoomCode}' sudah digunakan." });

        room.Name     = dto.Name;
        room.RoomCode = dto.RoomCode;
        room.Capacity = dto.Capacity;
        room.Building = dto.Building;
        room.Floor    = dto.Floor;
        room.IsActive = dto.IsActive;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    // ─────────────────────────────────────────────
    // DELETE /api/rooms/{id}   →  Soft Delete
    // ─────────────────────────────────────────────
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var room = await _context.Rooms.FindAsync(id);
        if (room == null)
            return NotFound(new { message = $"Ruangan dengan id {id} tidak ditemukan." });

        // Cek apakah ada booking aktif di ruangan ini
        var hasActiveBooking = await _context.Bookings
            .AnyAsync(b => b.RoomId == id && b.Status == "Pending");
        if (hasActiveBooking)
            return BadRequest(new { message = "Ruangan masih memiliki booking Pending dan tidak bisa dihapus." });

        room.DeletedAt = DateTime.UtcNow;
        room.IsActive  = false;
        await _context.SaveChangesAsync();

        return Ok(new { message = "Ruangan berhasil dihapus." });
    }
}
