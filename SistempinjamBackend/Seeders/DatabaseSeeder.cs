using SistempinjamBackend.Data;
using SistempinjamBackend.Models;

namespace SistempinjamBackend.Seeders;

public static class DatabaseSeeder
{
    public static void Seed(AppDbContext context)
    {
        // Seed rooms jika belum ada data
        if (!context.Rooms.Any())
        {
            var rooms = new List<Room>
            {
                new Room { Name = "Ruang Rapat A",   RoomCode = "RR-A",  Capacity = 20,  Building = "Gedung A", Floor = "1" },
                new Room { Name = "Ruang Rapat B",   RoomCode = "RR-B",  Capacity = 15,  Building = "Gedung A", Floor = "2" },
                new Room { Name = "Aula Utama",       RoomCode = "AULA",  Capacity = 200, Building = "Gedung B", Floor = "1" },
                new Room { Name = "Lab Komputer 1",  RoomCode = "LAB-1", Capacity = 40,  Building = "Gedung C", Floor = "1" },
                new Room { Name = "Ruang Seminar",   RoomCode = "SEM-1", Capacity = 80,  Building = "Gedung B", Floor = "2" },
            };
            context.Rooms.AddRange(rooms);
            context.SaveChanges();
        }

        // Seed bookings jika belum ada data
        if (!context.Bookings.Any())
        {
            var rooms = context.Rooms.ToList();
            var now = DateTime.UtcNow;

            var bookings = new List<Booking>
            {
                new Booking
                {
                    BorrowerName  = "Ahmad Fauzi",
                    PurposeOfUse  = "Rapat Koordinasi Tim A",
                    StartTime     = now.AddDays(1).Date.AddHours(9),
                    EndTime       = now.AddDays(1).Date.AddHours(11),
                    Status        = "Approved",
                    RoomId        = rooms[0].Id,
                    Notes         = "Mohon siapkan proyektor"
                },
                new Booking
                {
                    BorrowerName  = "Siti Rahayu",
                    PurposeOfUse  = "Seminar Nasional Informatika",
                    StartTime     = now.AddDays(2).Date.AddHours(8),
                    EndTime       = now.AddDays(2).Date.AddHours(16),
                    Status        = "Pending",
                    RoomId        = rooms[2].Id,
                },
                new Booking
                {
                    BorrowerName  = "Budi Santoso",
                    PurposeOfUse  = "Praktikum Algoritma",
                    StartTime     = now.AddDays(3).Date.AddHours(13),
                    EndTime       = now.AddDays(3).Date.AddHours(15),
                    Status        = "Approved",
                    RoomId        = rooms[3].Id,
                },
                new Booking
                {
                    BorrowerName  = "Dewi Lestari",
                    PurposeOfUse  = "Workshop UI/UX Design",
                    StartTime     = now.AddDays(4).Date.AddHours(10),
                    EndTime       = now.AddDays(4).Date.AddHours(14),
                    Status        = "Rejected",
                    RoomId        = rooms[1].Id,
                    Notes         = "Ditolak karena bentrok jadwal"
                },
                new Booking
                {
                    BorrowerName  = "Rizki Pratama",
                    PurposeOfUse  = "Presentasi Tugas Akhir",
                    StartTime     = now.AddDays(5).Date.AddHours(9),
                    EndTime       = now.AddDays(5).Date.AddHours(12),
                    Status        = "Pending",
                    RoomId        = rooms[4].Id,
                },
            };
            context.Bookings.AddRange(bookings);
            context.SaveChanges();
        }
    }
}
