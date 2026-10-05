using System.Reflection;
using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace AutoHub.API.Extensions;

/// <summary>
/// Configura o Swagger/OpenAPI para exibir documentos separados por versão da API.
/// </summary>
public static class SwaggerExtensions
{
    public static IServiceCollection AddAutoHubSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        services.AddSwaggerGen(options =>
        {
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

/// <summary>
/// Gera um documento Swagger para cada versão descoberta pelo ApiExplorer.
/// </summary>
public class ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
    : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider = provider;

    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in _provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
        }
    }

    private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
    {
        var info = new OpenApiInfo
        {
            Title = "AutoHub API — Concessionária & Oficina Mecânica",
            Version = description.ApiVersion.ToString(),
            Description = "API REST corporativa para gestão integrada de concessionária de veículos e oficina mecânica.",
            Contact = new OpenApiContact
            {
                Name = "Equipe AutoHub (Matheus Roque RM: 561959 / Giovane dos Santos RM: 561336)",
                Url = new Uri("https://github.com/roque-arantes/AutoHub")
            }
        };

        if (description.IsDeprecated)
        {
            info.Description += " ⚠️ ESTA VERSÃO ESTÁ DEPRECADA. Migre para a versão mais recente.";
        }

        return info;
    }
}
