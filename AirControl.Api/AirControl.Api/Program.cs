using Microsoft.EntityFrameworkCore;
using AirControl.Api.Data;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// ===================== CORS =====================
const string CorsPolicy = "AirControlCors";

var allowedOrigins = new[]
{
    "http://127.0.0.1:5500",
    "http://localhost:5500",
    "https://aircontrolos-web.vercel.app"   // <-- SEU SITE NO VERCEL
};

builder.Services.AddCors(options =>
{
    options.AddPolicy(CorsPolicy, policy =>
    {
        policy
            .WithOrigins(allowedOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod();
        // Se um dia usar cookie/autenticação por cookie, aí sim:
        // .AllowCredentials();
    });
});

// ===================== CONTROLLERS / JSON =====================
builder.Services.AddControllers()
    .AddJsonOptions(o =>
    {
        o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ===================== BANCO DE DADOS =====================
if (builder.Environment.IsDevelopment())
{
    // DEV → SQL Server local
    builder.Services.AddDbContext<AppDbContext>(opts =>
        opts.UseSqlServer(
            builder.Configuration.GetConnectionString("DefaultConnection")));
}
else
{
    // PRODUÇÃO (Render) → PostgreSQL
    builder.Services.AddDbContext<AppDbContext>(opts =>
        opts.UseNpgsql(
            builder.Configuration.GetConnectionString("DefaultConnection")));
}

// ===================== APP =====================
var app = builder.Build();

// aplica migrations automaticamente
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// CORS ANTES DOS CONTROLLERS
app.UseCors(CorsPolicy);

app.MapControllers();

// opcional, mas ajuda o Render no health-check
app.MapGet("/healthz", () => Results.Ok("OK"));

app.Run();
