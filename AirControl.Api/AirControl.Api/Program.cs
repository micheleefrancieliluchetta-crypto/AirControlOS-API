using AirControl.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Http;

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

// Controllers
builder.Services.AddControllers();

var app = builder.Build();

// ================================
// MIDDLEWARE DE CORS MANUAL
// ================================
app.Use(async (context, next) =>
{
    // Origem que está chamando (Vercel)
    var origin = context.Request.Headers["Origin"].ToString();

    // Se quiser liberar só o Vercel, troca "*" por "https://aircontrolos-web.vercel.app"
    context.Response.Headers["Access-Control-Allow-Origin"] =
        string.IsNullOrEmpty(origin) ? "*" : origin;

    context.Response.Headers["Vary"] = "Origin";
    context.Response.Headers["Access-Control-Allow-Credentials"] = "true";
    context.Response.Headers["Access-Control-Allow-Headers"] = "Content-Type, Authorization";
    context.Response.Headers["Access-Control-Allow-Methods"] = "GET, POST, PUT, DELETE, OPTIONS";

    // Se for preflight (OPTIONS), já responde aqui
    if (context.Request.Method == HttpMethods.Options)
    {
        context.Response.StatusCode = StatusCodes.Status200OK;
        await context.Response.CompleteAsync();
        return;
    }

    await next();
});

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

app.UseAuthorization();

app.MapControllers();

// Rotas simples
app.MapGet("/", () => Results.Ok("API AirControlOS funcionando 🚀"))
   .AllowAnonymous();

app.MapGet("/healthz", () => Results.Ok("ok"))
   .AllowAnonymous();

app.Run();
