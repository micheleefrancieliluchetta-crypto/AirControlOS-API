using AirControl.Api.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ================================
// CONFIGURAÇÃO DE SERVIÇOS
// ================================

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext (PostgreSQL no Render)
builder.Services.AddDbContext<AppDbContext>(options =>
{
    // Usa a connection string DefaultConnection
    var connStr = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseNpgsql(connStr);
});

// Controllers
builder.Services.AddControllers();

// CORS – libera o frontend da Vercel e o localhost
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

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    // Swagger em produção também
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "AirControl.Api v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseStaticFiles();

app.UseRouting();

// aplica a policy de CORS
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
