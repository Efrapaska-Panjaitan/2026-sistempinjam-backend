using Microsoft.EntityFrameworkCore;
using SistempinjamBackend.Data;
using SistempinjamBackend.Seeders;

var builder = WebApplication.CreateBuilder(args);

// ─── Services ──────────────────────────────────────────────────────────────

// 1. Database: PostgreSQL via Entity Framework Core
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Controllers
builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        // Gunakan validasi bawaan ModelState (DataAnnotations)
        // Ini sudah aktif secara default, tapi dituliskan agar jelas
        options.SuppressModelStateInvalidFilter = false;
    });

// 3. Swagger / OpenAPI documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title       = "SISTEMPINJAM API",
        Version     = "v1",
        Description = "REST API untuk Sistem Peminjaman Ruangan Kampus"
    });
});

// 4. CORS: izinkan request dari frontend React (localhost:5173)
var allowedOrigins = builder.Configuration["AllowedOrigins"] ?? "http://localhost:5173";
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(allowedOrigins.Split(','))
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// ─── Middleware Pipeline ────────────────────────────────────────────────────

// Jalankan migration otomatis saat startup (development only)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();          // Jalankan semua migration yang belum diapply
    DatabaseSeeder.Seed(db);        // Isi data awal jika tabel kosong
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "SISTEMPINJAM API v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseCors("AllowFrontend");
app.UseAuthorization();
app.MapControllers();

app.Run();
