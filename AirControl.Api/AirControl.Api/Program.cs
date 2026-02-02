using AirControl.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// ===================== CORS =====================
// Libera somente o frontend do Vercel
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .WithOrigins("https://sistemasmaxi.vercel.app")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// ===================== DB (PostgreSQL) =====================
builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connStr = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseNpgsql(connStr);
});

// ===================== CONTROLLERS =====================
builder.Services.AddControllers()
    .AddJsonOptions(o =>
    {
        // evita problemas com referência circular (EF)
        o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddEndpointsApiExplorer();

// ===================== SWAGGER =====================
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Maxi API",
        Version = "v1"
    });

    // JWT Bearer no Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Digite: Bearer {seu_token}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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

    // evita estouro se houver conflito de endpoints
    options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

    // evita conflito de schemas quando existem classes com mesmo nome em namespaces diferentes
    options.CustomSchemaIds(t => t.FullName);
});

var app = builder.Build();

// ===================== ERROS DE DEV =====================
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// ===================== MIGRATIONS AUTOMÁTICAS =====================
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    try
    {
        db.Database.Migrate();
        Console.WriteLine("Migrations aplicadas com sucesso.");
    }
    catch (Exception ex)
    {
        Console.WriteLine("Erro ao aplicar migrations: " + ex.Message);
    }
}

// ===================== PIPELINE (ORDEM CERTA) =====================
app.UseSwagger();
app.UseSwaggerUI();

// 1) Routing
app.UseRouting();

// 2) CORS (TEM QUE SER AQUI, antes do auth e do MapControllers)
app.UseCors("AllowAll");

// 3) Auth
app.UseAuthentication();
app.UseAuthorization();

// 4) Controllers
app.MapControllers();

// ===================== HEALTH =====================
app.MapGet("/healthz", () => Results.Ok("ok"));

app.MapGet("/health", async (AppDbContext db) =>
{
    try
    {
        var ok = await db.Database.CanConnectAsync();
        return ok ? Results.Ok("DB OK") : Results.Problem("FALHA NO BANCO DE DADOS");
    }
    catch (Exception ex)
    {
        return Results.Problem("Erro ao conectar no banco: " + ex.Message);
    }
});

app.Run();

