using ECommerceApi.Data;
using ECommerceApi.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

var builder = WebApplication.CreateBuilder(args);

// 1. REGISTRO DE SERVICIOS EN EL CONTENEDOR (IServiceCollection)

// Configuración robusta de SQL Server con estrategia de Resiliencia ante fallos transitorios
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null
        )
    ));

builder.Services.AddControllers();

// 💡 SOLUCIÓN AL ERROR: Registramos explícitamente los servicios de autorización requeridos por la plataforma
builder.Services.AddAuthorization(); 

builder.Services.AddEndpointsApiExplorer();

// Soporte de documentación OpenAPI nativa (Estándar para .NET 9/10)
builder.Services.AddOpenApi(); 

var app = builder.Build();

// 2. CONFIGURACIÓN DEL PIPELINE DE PETICIONES (IApplicationBuilder / Middleware)

// Middleware Global de excepciones (Siempre debe ser el primero para atrapar todos los fallos)
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Activación del middleware de autorización en orden jerárquico correcto
app.UseAuthorization(); 

app.MapControllers();

app.Run();
