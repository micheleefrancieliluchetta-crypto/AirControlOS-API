using AirControl.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// ================== CORS ==================
const string CorsPolicyName = "AllowAirControlWeb";

builder.Services.AddCors(options =>
{
    options.AddPolicy(CorsPolicyName, policy =>
    {
        policy
            // ORIGENS QUE PODEM CHAMAR A API
            .WithOrigins(
                "https://aircontrolos-web.vercel.app", // Vercel produção
                "https://aircontrolos-web-git-main-franciele-luchettas-projects.vercel.app", // preview
                "http://localhost:5500",               // dev
                "http://127.0.0.1:5500"               // dev
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
            // .AllowCredentials(); // só se você usar cookies/autenticação por cookie
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

var app = builder.Build();

// =============== PIPELINE HTTP ===============
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "AirControl.Api v1");
    c.RoutePrefix = "swagger";
});

app.UseStaticFiles();

app.UseRouting();

// *** CORS AQUI, DEPOIS DO UseRouting E ANTES DE AUTH ***
app.UseCors(CorsPolicyName);

// Se tiver JWT:
// app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

// Rotas simples de teste
app.MapGet("/", () => Results.Ok("API AirControlOS funcionando 🚀")).AllowAnonymous();
app.MapGet("/healthz", () => Results.Ok("ok")).AllowAnonymous();

app.Run();
