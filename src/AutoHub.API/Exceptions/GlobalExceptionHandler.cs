using AutoHub.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace AutoHub.API.Exceptions;

public class GlobalExceptionHandler(
    ILogger<GlobalExceptionHandler> logger,
    IHostEnvironment env) : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger = logger;
    private readonly IHostEnvironment _env = env;

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var traceId = httpContext.TraceIdentifier;

        _logger.LogError(
            exception,
            "Exceção capturada no pipeline para a requisição {Path} | TraceId: {TraceId}",
            httpContext.Request.Path,
            traceId);

        var (statusCode, title, detail) = MapException(exception);

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail,
            Instance = httpContext.Request.Path
        };

        problemDetails.Extensions["traceId"] = traceId;

        if (_env.IsDevelopment() && statusCode == StatusCodes.Status500InternalServerError)
        {
            problemDetails.Extensions["exception"] = exception.ToString();
        }

        httpContext.Response.StatusCode = statusCode;
        httpContext.Response.ContentType = "application/problem+json";

        await httpContext.Response.WriteAsJsonAsync(
            problemDetails,
            options: (System.Text.Json.JsonSerializerOptions?)null,
            contentType: "application/problem+json",
            cancellationToken: cancellationToken);

        return true;
    }

    private (int StatusCode, string Title, string Detail) MapException(Exception exception)
    {
        return exception switch
        {
            ResourceNotFoundException ex => (
                StatusCodes.Status404NotFound,
                "Recurso não encontrado",
                ex.Message
            ),
            KeyNotFoundException ex => (
                StatusCodes.Status404NotFound,
                "Recurso não encontrado",
                ex.Message
            ),
            ConflictException ex => (
                StatusCodes.Status409Conflict,
                "Conflito de dados",
                ex.Message
            ),
            DomainException ex => (
                StatusCodes.Status400BadRequest,
                "Violação de regra de negócio",
                ex.Message
            ),
            ArgumentException ex => (
                StatusCodes.Status400BadRequest,
                "Parâmetro inválido",
                ex.Message
            ),
            _ => (
                StatusCodes.Status500InternalServerError,
                "Erro interno no servidor",
                _env.IsDevelopment()
                    ? exception.Message
                    : "Ocorreu um erro interno inesperado ao processar a requisição. Contate o suporte com o identificador traceId."
            )
        };
    }
}
