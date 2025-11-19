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
    // Usa a connection string DefaultConnection (Render pega de variável de ambiente ou appsettings)
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
                "https://aircontrolos-web.vercel.app", // frontend em produção
                "http://localhost:5500",               // teste local (Live Server)
                "http://127.0.0.1:5500"
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
        // Se algum dia usar cookies/autenticação com credenciais:
        // .AllowCredentials();
    });
});

var app = builder.Build();

// ================================
// PIPELINE HTTP
// ================================

// NADA de app.UseHttpsRedirection(); no Render
// app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    // Swagger também em produção, na rota /swagger
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "AirControl.Api v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseStaticFiles();

app.UseRouting();

// *** AQUI aplica a policy de CORS ***
app.UseCors("AllowFrontend");

app.UseAuthorization();

app.MapControllers();

// Endpoint simples de health check (usado pelo Render ou por você)
app.MapGet("/healthz", () => Results.Ok("ok"))
   .AllowAnonymous();

app.Run();
