using AirControl.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// =============== CORS: Libera para qualquer origem (útil em dev ou testes) ===============
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
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
            new string[] {}
        }
    });
});

var app = builder.Build();

// =============== MIGRAÇÕES ===============
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

// =============== PIPELINE ===============
app.UseSwagger();
app.UseSwaggerUI();

// ✅ ATIVA O CORS antes da autenticação
app.UseCors("AllowAll");

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// =============== HEALTH ===================
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

// =============== CONTROLLERS ===============
app.MapControllers();

app.Run();

