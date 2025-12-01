using AirControl.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// ================================
// CONFIGURAÇÃO DE SERVIÇOS
// ================================

// Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "AirControl.Api",
        Version = "v1"
    });

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
    var connStr = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseNpgsql(connStr);
});

// Controllers (API tradicional)
builder.Services.AddControllers();

// ================================
// CORS – policy única liberando geral
// ================================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .AllowAnyOrigin()  // libera qualquer origem (Vercel, localhost, etc.)
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// ================================
// PIPELINE HTTP
// ================================

// Swagger sempre disponível (se quiser, pode limitar para Development)
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "AirControl.Api v1");
    c.RoutePrefix = "swagger";
});

// Arquivos estáticos (se tiver)
app.UseStaticFiles();

// Roteamento
app.UseRouting();

// CORS TEM QUE VIR AQUI:
// entre UseRouting e UseAuthorization
app.UseCors("AllowAll");

// Autorização (JWT etc., quando você usar)
app.UseAuthorization();

// Controllers
app.MapControllers();

// Rotas simples (health check, root etc.)
app.MapGet("/", () => Results.Ok("API AirControlOS funcionando 🚀"))
   .AllowAnonymous();

app.MapGet("/healthz", () => Results.Ok("ok"))
   .AllowAnonymous();

app.Run();

