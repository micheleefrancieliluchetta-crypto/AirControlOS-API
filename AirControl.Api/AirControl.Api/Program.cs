using AirControl.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// ================================
// CONFIGURAÇÃO DE SERVIÇOS
// ================================

// Swagger
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "AirControl.Api",
        Version = "v1"
    });

    // CONFIGURAÇÃO DO TOKEN NO SWAGGER 🔒
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Insira o token JWT assim: Bearer {seu_token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// DbContext (PostgreSQL no Render)
builder.Services.AddDbContext<AppDbContext>(options =>
{
    // Usa a connection string DefaultConnection
    var connStr = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseNpgsql(connStr);
});

// Controllers
builder.Services.AddControllers();

// ================================
// CORS – LIBERA FRONTEND (Vercel, localhost, etc.)
// ================================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .WithOrigins(
                "https://aircontrolos-web.vercel.app", // produção
                "http://localhost:5500",               // teste local
                "http://127.0.0.1:5500"
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// ================================
// PIPELINE HTTP
// ================================

// NÃO usar HTTPS redirection no Render
// app.UseHttpsRedirection();

// Swagger em DEV e em PRODUÇÃO
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "AirControl.Api v1");
    c.RoutePrefix = "swagger";
});

app.UseStaticFiles();

app.UseRouting();

// aplica CORS usando a policy "AllowFrontend"
app.UseCors("AllowFrontend");

app.UseAuthorization();

app.MapControllers();

// ============================================
// ROTAS SIMPLES: / (home) e /healthz
// ============================================

// Rota inicial personalizada
app.MapGet("/", () => Results.Ok("API AirControlOS funcionando 🚀"))
   .AllowAnonymous();

// Health check
app.MapGet("/healthz", () => Results.Ok("ok"))
   .AllowAnonymous();

app.Run();
