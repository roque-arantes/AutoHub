using System.Text.Json.Serialization;
using AutoHub.API.Exceptions;
using AutoHub.API.Extensions;
using AutoHub.Application.Interfaces;
using AutoHub.Application.Services;
using AutoHub.Infrastructure.Data;
using AutoHub.Infrastructure.Repositories;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.RateLimiting;
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

// Versionamento da API (CP5 — seção A)
builder.Services
    .AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(2, 0);
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.ReportApiVersions = true;
        options.ApiVersionReader = ApiVersionReader.Combine(
            new QueryStringApiVersionReader("api-version"),
            new HeaderApiVersionReader("X-Api-Version"),
            new UrlSegmentApiVersionReader());
    })
    .AddMvc()
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });

// Documentação Swagger / OpenAPI com versionamento (CP5)
builder.Services.AddAutoHubSwagger();

// Rate Limiting (CP5 — seção C)
builder.Services.AddAutoHubRateLimiting();

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

// 2. Rate Limiter (depois de UseExceptionHandler, antes de MapControllers)
app.UseRateLimiter();

// 3. Swagger UI com suporte a múltiplas versões
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
    foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
    {
        var url = $"/swagger/{description.GroupName}/swagger.json";
        var name = description.IsDeprecated
            ? $"AutoHub API {description.GroupName} (DEPRECADA)"
            : $"AutoHub API {description.GroupName}";
        options.SwaggerEndpoint(url, name);
    }
    options.RoutePrefix = "swagger";
});

// 4. Aplicação automática das migrations do CP2 na inicialização
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}

// 5. Mapeamento de Controllers
app.MapControllers();

// 6. Health check com relatório JSON completo (processo self + banco de dados)
// DisableRateLimiting garante que /health nunca é limitado pelo rate limiter
app.MapAutoHubHealthChecks().DisableRateLimiting();

app.Run();
