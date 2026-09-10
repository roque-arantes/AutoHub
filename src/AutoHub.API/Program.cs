using System.Text.Json.Serialization;
using AutoHub.API.Exceptions;
using AutoHub.API.Extensions;
using AutoHub.Application.Interfaces;
using AutoHub.Application.Services;
using AutoHub.Infrastructure.Data;
using AutoHub.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Controllers com suporte a JSON
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

// Tratamento global de exceções (RFC 7807)
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// Documentação Swagger / OpenAPI com XML Comments
builder.Services.AddAutoHubSwagger();

// Health Checks (disponibilidade operacional da API e banco de dados)
builder.Services.AddAutoHubHealthChecks();

// Persistência com EF Core (SQLite)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("Default"));
});

// Repositório genérico
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// Serviços de aplicação (Clean Architecture)
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<IMarcaService, MarcaService>();
builder.Services.AddScoped<IVeiculoEstoqueService, VeiculoEstoqueService>();
builder.Services.AddScoped<IDataSeeder, DataSeeder>();

var app = builder.Build();

// 1. Pipeline de tratamento global de exceções
app.UseExceptionHandler();

// 2. Swagger UI
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "AutoHub API v1");
    c.RoutePrefix = "swagger";
});

// 3. Aplicação automática das migrations do CP2 na inicialização
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}

// 4. Mapeamento de Controllers
app.MapControllers();

// 5. Health check com relatório JSON completo (processo self + banco de dados)
app.MapAutoHubHealthChecks();

app.Run();
