using Microsoft.EntityFrameworkCore;
using AirControl.Api.Data;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// CORS (igual você já fez)
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

// Controllers + JSON
builder.Services.AddControllers()
    .AddJsonOptions(o =>
    {
        o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// ************* CONFIGURAÇÃO DO BANCO *************
if (builder.Environment.IsDevelopment())
{
    // Quando rodar no Visual Studio (ambiente Development) → SQL Server local
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
    // Quando rodar no Render (Production) → PostgreSQL
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
}
// ***********************************************

var app = builder.Build();

// aplica migrations automaticamente (opcional, mas ajuda)
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
else
{
    app.UseHttpsRedirection();
}

app.UseCors(CorsPolicy);

app.MapControllers();

app.Run();
