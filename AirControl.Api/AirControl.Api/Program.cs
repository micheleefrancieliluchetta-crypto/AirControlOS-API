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
        // Se você NÃO usa cookies, NÃO use AllowCredentials()
    });
});

// ===================== DB (PostgreSQL) =====================
builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connStr = builder.Configuration.GetConnectionString("DefaultConnection");

    if (string.IsNullOrWhiteSpace(connStr))
        throw new Exception("Connection string 'DefaultConnection' NÃO encontrada. Verifique ConnectionStrings__DefaultConnection no Render.");

    options.UseNpgsql(connStr);
});

// ===================== CONTROLLERS =====================
builder.Services.AddControllers()
    .AddJsonOptions(o =>
    {
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

    options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
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

// ===================== SWAGGER =====================
app.UseSwagger();
app.UseSwaggerUI();

// ===================== PIPELINE (ORDEM CERTA) =====================
app.UseRouting();

// ✅ CORS precisa estar aqui
app.UseCors("AllowAll");

// ✅ “Blindagem” do preflight: responde OPTIONS para qualquer rota
app.MapMethods("{*path}", new[] { "OPTIONS" }, () => Results.Ok())
   .RequireCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

// Controllers
app.MapControllers(); // não precisa RequireCors aqui, pois já tem UseCors()

// ===================== HEALTH =====================
app.MapGet("/healthz", () => Results.Ok("ok"))
   .RequireCors("AllowAll");

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
}).RequireCors("AllowAll");

app.Run();
