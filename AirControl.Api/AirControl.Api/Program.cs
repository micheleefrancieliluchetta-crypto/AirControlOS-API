using AirControl.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);

// =============== CORS ===============
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendMaxi", policy =>
    {
        policy
            .WithOrigins(
                "https://sistemasmaxi.vercel.app",
                "https://sistemasmaxi.vercel.app/"  // só por garantia
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
            // Se em algum momento usar cookies/token em cookie:
            // .AllowCredentials();
    });
});

// =============== DB (PostgreSQL) ===============
builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connStr = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseNpgsql(connStr);
});

// =============== CONTROLLERS + SWAGGER ===============
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "AirControlOS API", Version = "v1" });

    // --- Segurança Bearer no Swagger ---
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Digite: Bearer {seu_token}"
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

    // --- Corrige conflito de rotas no Swagger (ambiguous HTTP method) ---
    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
});

var app = builder.Build();

// =============== ERROS DE DEV ===============
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// =============== MIGRATIONS AUTOMÁTICAS ===============
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

// =============== PIPELINE HTTP ===============
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

// >>> ORDEM IMPORTANTE <<<
app.UseRouting();                 // 1️⃣ resolve a rota
app.UseCors("FrontendMaxi");      // 2️⃣ aplica CORS nessa rota
app.UseAuthentication();          // 3️⃣ auth
app.UseAuthorization();           // 4️⃣ autorização

app.MapControllers();             // 5️⃣ controllers

// Endpoints de saúde (opcional)
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
