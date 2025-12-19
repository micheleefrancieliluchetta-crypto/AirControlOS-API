using AirControl.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// =================== CORS ===================
const string CorsPolicyName = "AllowAirControlWeb";

builder.Services.AddCors(options =>
{
    options.AddPolicy(CorsPolicyName, policy =>
    {
        policy
            .WithOrigins(
                "https://aircontrolos-web.vercel.app", // FRONT em produção (Vercel)
                "http://localhost:5500",               // FRONT em dev
                "http://127.0.0.1:5500"
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
            // Se um dia usar cookies/sessão:
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

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

// *** CORS TEM QUE FICAR AQUI ***
app.UseCors(CorsPolicyName);

// Se tiver autenticação JWT, entra aqui
// app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

// Rotas simples
app.MapGet("/", () => Results.Ok("API AirControlOS funcionando 🚀")).AllowAnonymous();
app.MapGet("/healthz", () => Results.Ok("ok")).AllowAnonymous();

app.Run();
