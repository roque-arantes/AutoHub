using System.Text.Json;
using AutoHub.Infrastructure.Data;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace AutoHub.API.Extensions;

public static class HealthCheckExtensions
{
    public static IServiceCollection AddAutoHubHealthChecks(this IServiceCollection services)
    {
        services.AddHealthChecks()
            .AddCheck("self", () => HealthCheckResult.Healthy("A API está operando normalmente."), tags: ["live"])
            .AddDbContextCheck<ApplicationDbContext>(
                name: "database",
                failureStatus: HealthStatus.Unhealthy,
                tags: ["ready", "db"]);

        return services;
    }

    public static IEndpointConventionBuilder MapAutoHubHealthChecks(this WebApplication app)
    {
        var isDevelopment = app.Environment.IsDevelopment();

        var options = new HealthCheckOptions
        {
            ResultStatusCodes =
            {
                [HealthStatus.Healthy] = StatusCodes.Status200OK,
                [HealthStatus.Degraded] = StatusCodes.Status200OK,
                [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
            },
            ResponseWriter = async (context, report) =>
            {
                context.Response.ContentType = "application/json; charset=utf-8";

                var response = new
                {
                    status = report.Status.ToString(),
                    totalDuration = report.TotalDuration.ToString(),
                    checks = report.Entries.Select(entry => new
                    {
                        name = entry.Key,
                        status = entry.Value.Status.ToString(),
                        duration = entry.Value.Duration.ToString(),
                        description = entry.Value.Description,
                        tags = entry.Value.Tags,
                        data = entry.Value.Data.Count > 0 ? entry.Value.Data : null,
                        exception = isDevelopment && entry.Value.Exception != null
                            ? entry.Value.Exception.Message
                            : null
                    })
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(response, new JsonSerializerOptions
                {
                    WriteIndented = true,
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }));
            }
        };

        return app.MapHealthChecks("/health", options);
    }
}
