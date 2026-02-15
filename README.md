# 2026-SISTEMPINJAM-BACKEND

REST API untuk **Sistem Peminjaman Ruang Kampus (SIPERU)**.

## Description

Backend API yang dibangun dengan ASP.NET Core 8.0 dan PostgreSQL, menyediakan endpoint untuk mengelola data ruangan dan peminjaman ruangan kampus secara terpusat.

## Features

- CRUD Ruangan (Room) — tambah, lihat, edit, hapus (soft delete)
- CRUD Peminjaman (Booking) — tambah, lihat, edit, hapus (soft delete)
- Pengelolaan Status Peminjaman — Pending / Approved / Rejected
- Validasi input dengan DataAnnotations
- Pencarian & Filter data
- Paginasi hasil daftar
- Cek konflik jadwal peminjaman secara otomatis
- Swagger UI untuk dokumentasi & testing API

## Tech Stack

- **Runtime**: .NET 8.0 (C#)
- **Framework**: ASP.NET Core Web API
- **ORM**: Entity Framework Core 8
- **Database**: PostgreSQL 15+
- **Documentation**: Swagger / OpenAPI

## Installation

```bash
# 1. Clone repository
git clone https://github.com/username/2026-sistempinjam-backend.git
cd 2026-sistempinjam-backend/2026-sistempinjam-backend

# 2. Buat database di PostgreSQL
# Buka psql lalu jalankan:
# CREATE DATABASE sistempinjam_db;

# 3. Konfigurasi koneksi database
# Salin template konfigurasi
cp ../appsettings.Development.example.json appsettings.Development.json
# Edit appsettings.Development.json → isi Password sesuai PostgreSQL lokal kamu

# 4. Restore dependencies
dotnet restore

# 5. Jalankan migration & seeder (otomatis saat startup)
dotnet run
```

## Environment Variables

Buat file `appsettings.Development.json` (sudah ada di .gitignore) dengan isi:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=sistempinjam_db;Username=postgres;Password=PASSWORDKAMU"
  }
}
```

## Usage

Setelah `dotnet run`, akses:

- **Swagger UI**: http://localhost:5000/swagger
- **Base API URL**: http://localhost:5000/api

### Endpoint Utama

| Method | URL | Deskripsi |
|--------|-----|-----------|
| GET | /api/bookings | Daftar semua booking (support search, status, pagination) |
| GET | /api/bookings/{id} | Detail satu booking |
| POST | /api/bookings | Buat booking baru |
| PUT | /api/bookings/{id} | Edit booking |
| PATCH | /api/bookings/{id}/status | Ubah status booking |
| DELETE | /api/bookings/{id} | Hapus booking (soft delete) |
| GET | /api/rooms | Daftar semua ruangan |
| GET | /api/rooms/{id} | Detail satu ruangan |
| POST | /api/rooms | Tambah ruangan |
| PUT | /api/rooms/{id} | Edit ruangan |
| DELETE | /api/rooms/{id} | Hapus ruangan (soft delete) |

## License

MIT
