using AirControl.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// ================================
// DB (PostgreSQL no Render)
// ================================
builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connStr = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseNpgsql(connStr);
});

// ================================
// CONTROLLERS + SWAGGER
// ================================
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

// ================================
// CORS GLOBAL (vale pra toda API)
// ================================
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .WithOrigins(
                "https://aircontrolos-web.vercel.app",
                "https://aircontrolos-web-git-main-franciele-luchettas-projects.vercel.app"
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
        // se algum dia usar cookies/autorização de browser:
        // .AllowCredentials();
    });
});

var app = builder.Build();

// ================================
// PIPELINE HTTP
// ================================
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "AirControl.Api v1");
    c.RoutePrefix = "swagger";
});

app.UseStaticFiles();

app.UseRouting();

// CORS global – DEPOIS de UseRouting e ANTES de UseAuthorization
app.UseCors();

app.UseAuthorization();

app.MapControllers();

// Rotas simples
app.MapGet("/", () => Results.Ok("API AirControlOS funcionando 🚀"))
   .AllowAnonymous();

app.MapGet("/healthz", () => Results.Ok("ok"))
   .AllowAnonymous();

app.Run();
