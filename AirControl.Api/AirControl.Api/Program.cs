using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// Swagger (útil para testar no Render)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ✅ CORS (Vercel + Locais)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .WithOrigins(
                "https://aircontrolos-web.vercel.app",
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
            // ⚠️ Só use AllowCredentials se você usa cookies/autenticação via navegador.
            // .AllowCredentials();
    });
});

var app = builder.Build();

// ✅ Render / Proxy: garante que https e headers sejam interpretados corretamente atrás do load balancer
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

// (Opcional) força https em produção
app.UseHttpsRedirection();

// ✅ CORS TEM QUE VIR ANTES de Authorization e antes de MapControllers
app.UseCors("AllowFrontend");

// Se você tiver autenticação, deixe assim. Se não tiver, não atrapalha.
app.UseAuthorization();

if (app.Environment.IsDevelopment() || app.Environment.EnvironmentName == "Production")
{
    // Se quiser deixar Swagger só em dev, troque para: if (app.Environment.IsDevelopment())
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();

