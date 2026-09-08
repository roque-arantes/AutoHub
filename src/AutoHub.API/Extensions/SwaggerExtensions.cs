using System.Reflection;
using Microsoft.OpenApi.Models;

namespace AutoHub.API.Extensions;

public static class SwaggerExtensions
{
    public static IServiceCollection AddAutoHubSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "AutoHub API — Concessionária & Oficina Mecânica",
                Version = "v1",
                Description = "API REST corporativa para gestão integrada de concessionária de veículos e oficina mecânica (FIAP CP3).",
                Contact = new OpenApiContact
                {
                    Name = "Equipe AutoHub (Matheus Roque RM: 561959 / Giovane dos Santos RM: 561336)",
                    Url = new Uri("https://github.com/roque-arantes/AutoHub")
                }
            });

            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
            if (File.Exists(xmlPath))
            {
                options.IncludeXmlComments(xmlPath);
            }
        });

        return services;
    }
}
