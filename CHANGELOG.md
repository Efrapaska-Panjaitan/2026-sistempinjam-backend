# Changelog

Semua perubahan penting akan didokumentasikan di file ini.
Format berdasarkan [Keep a Changelog](https://keepachangelog.com/en/1.0.0/)
dan menggunakan [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [1.0.0] - 2026-02-17

### Added
- Inisialisasi proyek ASP.NET Core 8.0 Web API
- Koneksi ke PostgreSQL menggunakan Entity Framework Core + Npgsql
- Konfigurasi Swagger UI untuk dokumentasi dan testing API
- Konfigurasi CORS untuk frontend React (localhost:5173)
- Auto migration database saat startup (dotnet ef database update)
- Auto seeding data awal saat startup
- File .gitignore untuk mengecualikan bin/, obj/, appsettings.Development.json
- File .env.example sebagai template konfigurasi
- File README.md dengan panduan instalasi lengkap
- Model `Room` dengan field: Name, RoomCode, Capacity, Building, Floor, IsActive, soft delete
- Model `Booking` dengan field: BorrowerName, PurposeOfUse, StartTime, EndTime, Status, Notes, soft delete
- `AppDbContext` dengan global query filter untuk soft delete
- Migration awal: tabel `rooms` dan `bookings`
- `DatabaseSeeder` dengan 5 data ruangan dan 5 data booking awal
- **RoomsController**: CRUD lengkap (GET list, GET by id, POST, PUT, DELETE soft delete)
- **BookingsController**: CRUD lengkap + PATCH status + validasi konflik jadwal
- DTO: `BookingCreateDto`, `BookingUpdateDto`, `BookingStatusDto`, `BookingResponseDto`
- DTO: `RoomCreateDto`, `RoomUpdateDto`, `RoomResponseDto`
- Validasi input via DataAnnotations (Required, MaxLength, Range, RegularExpression)
- CORS configuration untuk frontend React (localhost:5173)
- Swagger UI untuk dokumentasi API
- Search & filter pada GET /api/bookings (by name, purpose, status)
- Pagination pada GET /api/bookings
- Auto migration & seeding saat startup (development mode)
- `.gitignore` untuk ASP.NET Core
- `.env.example` sebagai template konfigurasi