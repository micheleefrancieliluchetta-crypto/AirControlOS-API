using Microsoft.EntityFrameworkCore;
using AirControl.Api.Data;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// ======================== CORS ========================
const string CorsPolicy = "AirControlCors";

var allowedOrigins = new[]
{
    "http://127.0.0.1:5500",
    "http://localhost:5500",
    "https://seu-projeto.vercel.app",
    "https://os.seudominio.com"
};

builder.Services.AddCors(opt =>
    opt.AddPolicy(CorsPolicy, p =>
        p.WithOrigins(allowedOrigins)
         .AllowAnyHeader()
         .AllowAnyMethod()
         .AllowCredentials()
    )
);

// =================== Controllers + JSON ===================
builder.Services.AddControllers()
    .AddJsonOptions(o =>
    {
        o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// =================== BANCO DE DADOS (PostgreSQL em tudo) ===================
builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseNpgsql(connString);
});

// =================== Build ===================
var app = builder.Build();

// =================== Aplicar migrations automaticamente ===================
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// =================== Pipeline ===================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHttpsRedirection();
}

app.UseCors(CorsPolicy);

app.MapControllers();

app.Run();
