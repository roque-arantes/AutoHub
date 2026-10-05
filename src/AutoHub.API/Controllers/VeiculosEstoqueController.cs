using AutoHub.Application.DTOs;
using AutoHub.Application.DTOs.VeiculosEstoque;
using AutoHub.Application.Interfaces;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace AutoHub.API.Controllers;

/// <summary>
/// Gerenciamento de veículos disponíveis para venda no estoque da concessionária.
/// Recurso versionado: v1 (deprecada) e v2 (atual, com paginação).
/// </summary>
[ApiController]
[Route("api/veiculos-estoque")]
[Route("api/v{version:apiVersion}/veiculos-estoque")]
[Produces("application/json")]
[ApiVersion("1.0", Deprecated = true)]
[ApiVersion("2.0")]
public class VeiculosEstoqueController(
    IVeiculoEstoqueService veiculoService,
    ILogger<VeiculosEstoqueController> logger) : ControllerBase
{
    private readonly IVeiculoEstoqueService _veiculoService = veiculoService;
    private readonly ILogger<VeiculosEstoqueController> _logger = logger;

    // ===================== V1 — Contrato antigo (deprecado) =====================

    /// <summary>
    /// [v1 — DEPRECADA] Lista todos os veículos em estoque (array sem paginação).
    /// </summary>
    /// <returns>Lista completa de veículos.</returns>
    /// <response code="200">Veículos retornados com sucesso (array).</response>
    [HttpGet]
    [MapToApiVersion("1.0")]
    [ProducesResponseType(typeof(IEnumerable<VeiculoEstoqueResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllV1()
    {
        var veiculos = await _veiculoService.GetAllAsync();
        return Ok(veiculos);
    }

    // ===================== V2 — Contrato novo (paginado) =====================

    /// <summary>
    /// [v2] Lista veículos em estoque com paginação.
    /// </summary>
    /// <param name="page">Número da página (padrão: 1, mínimo: 1).</param>
    /// <param name="pageSize">Itens por página (padrão: 20, de 1 a 100).</param>
    /// <returns>Envelope paginado com veículos.</returns>
    /// <response code="200">Página de veículos retornada com sucesso.</response>
    /// <response code="400">Parâmetros de paginação fora do intervalo permitido.</response>
    [HttpGet]
    [MapToApiVersion("2.0")]
    [ProducesResponseType(typeof(PagedResult<VeiculoEstoqueResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAllV2(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var result = await _veiculoService.GetPagedAsync(page, pageSize);
        return Ok(result);
    }

    // ===================== Endpoints comuns (v1 e v2) =====================

    /// <summary>
    /// Obtém um veículo do estoque pelo seu identificador único.
    /// </summary>
    /// <param name="id">Identificador único do veículo (GUID).</param>
    /// <returns>Dados do veículo encontrado.</returns>
    /// <response code="200">Veículo retornado com sucesso.</response>
    /// <response code="404">Veículo não encontrado.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(VeiculoEstoqueResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var veiculo = await _veiculoService.GetByIdAsync(id);
        return Ok(veiculo);
    }

    /// <summary>
    /// Cadastra um novo veículo no estoque da concessionária.
    /// </summary>
    /// <param name="dto">Dados do veículo para cadastro.</param>
    /// <returns>Veículo cadastrado no estoque.</returns>
    /// <response code="201">Veículo cadastrado com sucesso.</response>
    /// <response code="400">Dados inválidos fornecidos.</response>
    /// <response code="404">Modelo informado não existe.</response>
    /// <response code="409">Conflito: Já existe um veículo com o chassi informado.</response>
    /// <response code="429">Limite de requisições excedido.</response>
    [HttpPost]
    [EnableRateLimiting("fixed-post")]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status429TooManyRequests)]
    [ProducesResponseType(typeof(VeiculoEstoqueResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] VeiculoEstoqueRequestDto dto)
    {
        var traceId = HttpContext.TraceIdentifier;
        _logger.LogInformation(
            "Iniciando cadastro de veículo de estoque (Chassi: {Chassis}) | TraceId: {TraceId}",
            dto.Chassis,
            traceId);

        var criado = await _veiculoService.CreateAsync(dto);

        _logger.LogInformation(
            "Veículo {VeiculoId} cadastrado com sucesso no estoque | TraceId: {TraceId}",
            criado.Id,
            traceId);

        return CreatedAtAction(nameof(GetById), new { id = criado.Id }, criado);
    }

    /// <summary>
    /// Atualiza as informações de um veículo no estoque.
    /// </summary>
    /// <param name="id">Identificador único do veículo.</param>
    /// <param name="dto">Novos dados do veículo.</param>
    /// <returns>Veículo com dados atualizados.</returns>
    /// <response code="200">Veículo atualizado com sucesso.</response>
    /// <response code="400">Dados inválidos fornecidos.</response>
    /// <response code="404">Veículo ou Modelo não encontrado.</response>
    /// <response code="409">Conflito: Chassi já cadastrado em outro veículo.</response>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(VeiculoEstoqueResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(Guid id, [FromBody] VeiculoEstoqueRequestDto dto)
    {
        var traceId = HttpContext.TraceIdentifier;
        _logger.LogInformation(
            "Iniciando atualização do veículo de estoque {VeiculoId} | TraceId: {TraceId}",
            id,
            traceId);

        var atualizado = await _veiculoService.UpdateAsync(id, dto);

        _logger.LogInformation(
            "Veículo de estoque {VeiculoId} atualizado com sucesso | TraceId: {TraceId}",
            id,
            traceId);

        return Ok(atualizado);
    }

    /// <summary>
    /// Remove um veículo do estoque.
    /// </summary>
    /// <param name="id">Identificador único do veículo.</param>
    /// <returns>Sem conteúdo.</returns>
    /// <response code="204">Veículo removido com sucesso.</response>
    /// <response code="404">Veículo não encontrado.</response>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var traceId = HttpContext.TraceIdentifier;
        _logger.LogInformation(
            "Iniciando exclusão do veículo {VeiculoId} | TraceId: {TraceId}",
            id,
            traceId);

        await _veiculoService.DeleteAsync(id);

        _logger.LogInformation(
            "Veículo {VeiculoId} excluído com sucesso | TraceId: {TraceId}",
            id,
            traceId);

        return NoContent();
    }
}
