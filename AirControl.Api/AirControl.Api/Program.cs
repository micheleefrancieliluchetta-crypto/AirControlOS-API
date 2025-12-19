using AirControl.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

// =============== CORS ===============
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins, policy =>
    {
        policy
            .WithOrigins(
                "https://aircontrolos-web.vercel.app",
                "http://localhost:5500",
                "http://127.0.0.1:5500"
            )
            .AllowAnyHeader()
            .AllowAnyMethod();

        // Se um dia você passar token via cookie/sessão, aí sim:
        // .AllowCredentials();
        // (mas nesse caso NÃO pode usar AllowAnyOrigin)
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

    // Bearer Auth no Swagger (se você usa JWT)
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

// =============== PIPELINE (ORDEM IMPORTA!) ===============
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    // No Render você também pode deixar o Swagger ligado se quiser:
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

// ✅ AQUI é onde muita gente erra: CORS tem que ser aplicado ANTES do MapControllers
app.UseCors(MyAllowSpecificOrigins);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
