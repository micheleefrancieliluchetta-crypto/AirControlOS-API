using Microsoft.EntityFrameworkCore;
using AirControl.Api.Data;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// ========== CORS ==========
const string CorsPolicy = "AirControlCors";

var allowedOrigins = new[]
{
    "http://127.0.0.1:5500",
    "http://localhost:5500",
    "http://localhost:5173",
    "https://aircontrolos-web.vercel.app" // frontend no Vercel
};

builder.Services.AddCors(opt =>
    opt.AddPolicy(CorsPolicy, p =>
        p.WithOrigins(allowedOrigins)
         .AllowAnyHeader()
         .AllowAnyMethod()
         .AllowCredentials()
    )
);

// ========== CONTROLLERS + JSON ==========
builder.Services.AddControllers()
    .AddJsonOptions(o =>
    {
        o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ========== BANCO DE DADOS ==========
if (builder.Environment.IsDevelopment())
{
    // DEV → SQL Server local (igual já estava)
    builder.Services.AddDbContext<AppDbContext>(opts =>
        opts.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
            sql =>
            {
                sql.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                sql.CommandTimeout(60);
            })
    );
}
else
{
    // PRODUÇÃO (Render) → PostgreSQL
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
}

var app = builder.Build();

// Aplica migrations automaticamente
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// ========== MIDDLEWARE ==========
app.UseSwagger();      // <-- sempre habilitado (dev + produção)
app.UseSwaggerUI();    // <-- sempre habilitado

app.UseHttpsRedirection();

app.UseCors(CorsPolicy);

app.MapControllers();

app.Run();
