using System.Text.Json;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;

namespace AutoHub.API.Extensions;

/// <summary>
/// Configura Rate Limiting com política fixed window para endpoints de escrita.
/// </summary>
public static class RateLimitExtensions
{
    public static IServiceCollection AddAutoHubRateLimiting(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            // Política fixed window para POST/PUT: 10 requisições por minuto, particionada por IP
            options.AddFixedWindowLimiter("fixed-post", limiterOptions =>
            {
                limiterOptions.PermitLimit = 10;
                limiterOptions.Window = TimeSpan.FromMinutes(1);
                limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                limiterOptions.QueueLimit = 0;
            });

            options.OnRejected = async (context, cancellationToken) =>
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                context.HttpContext.Response.ContentType = "application/problem+json";

                var retryAfter = context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfterValue)
                    ? (int)retryAfterValue.TotalSeconds
                    : 60;

                context.HttpContext.Response.Headers.RetryAfter = retryAfter.ToString();

                var problemDetails = new
                {
                    type = "https://tools.ietf.org/html/rfc6585#section-4",
                    title = "Limite de requisições excedido",
                    status = 429,
                    detail = $"Você excedeu o limite de requisições. Tente novamente em {retryAfter} segundo(s).",
                    instance = context.HttpContext.Request.Path.ToString(),
                    retryAfterSeconds = retryAfter
                };

                await context.HttpContext.Response.WriteAsync(
                    JsonSerializer.Serialize(problemDetails, new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        WriteIndented = true
                    }),
                    cancellationToken);
            };
        });

        return services;
    }
}
