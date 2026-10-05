using AutoHub.Application.Interfaces;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace AutoHub.API.Controllers;

/// <summary>
/// Endpoint auxiliar para carga inicial de dados (Seed).
/// </summary>
[ApiController]
[ApiVersionNeutral]
[Route("api/[controller]")]
[Produces("application/json")]
public class SeedController(
    IDataSeeder dataSeeder,
    ILogger<SeedController> logger) : ControllerBase
{
    private readonly IDataSeeder _dataSeeder = dataSeeder;
    private readonly ILogger<SeedController> _logger = logger;

    /// <summary>
    /// Popula o banco de dados com dados iniciais de demonstração (marcas, modelos, clientes, veículos, etc.).
    /// </summary>
    /// <returns>Resultado da operação de carga inicial.</returns>
    /// <response code="201">Dados de exemplo inseridos com sucesso.</response>
    /// <response code="200">Os dados já haviam sido previamente inseridos.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Seed()
    {
        var traceId = HttpContext.TraceIdentifier;
        _logger.LogInformation("Executando operação de seed no banco de dados | TraceId: {TraceId}", traceId);

        var inserido = await _dataSeeder.SeedAsync();

        if (!inserido)
        {
            _logger.LogInformation("Seed ignorado: dados já existentes | TraceId: {TraceId}", traceId);
            return Ok(new { message = "Dados já foram inseridos previamente." });
        }

        _logger.LogInformation("Seed executado com sucesso | TraceId: {TraceId}", traceId);
        return Created("/api/seed", new { message = "Dados de exemplo inseridos com sucesso." });
    }
}
