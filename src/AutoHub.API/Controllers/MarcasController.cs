using AutoHub.Application.DTOs.Marcas;
using AutoHub.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AutoHub.API.Controllers;

/// <summary>
/// Gerenciamento de fabricantes e marcas de veículos.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class MarcasController(
    IMarcaService marcaService,
    ILogger<MarcasController> logger) : ControllerBase
{
    private readonly IMarcaService _marcaService = marcaService;
    private readonly ILogger<MarcasController> _logger = logger;

    /// <summary>
    /// Lista todas as marcas cadastradas.
    /// </summary>
    /// <returns>Lista de marcas.</returns>
    /// <response code="200">Marcas retornadas com sucesso.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<MarcaResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var marcas = await _marcaService.GetAllAsync();
        return Ok(marcas);
    }

    /// <summary>
    /// Obtém uma marca pelo seu identificador único.
    /// </summary>
    /// <param name="id">Identificador único da marca (GUID).</param>
    /// <returns>Dados da marca encontrada.</returns>
    /// <response code="200">Marca retornada com sucesso.</response>
    /// <response code="404">Marca não encontrada.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(MarcaResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var marca = await _marcaService.GetByIdAsync(id);
        return Ok(marca);
    }

    /// <summary>
    /// Cadastra uma nova marca de veículo.
    /// </summary>
    /// <param name="dto">Dados da marca a ser criada.</param>
    /// <returns>Marca cadastrada.</returns>
    /// <response code="201">Marca cadastrada com sucesso.</response>
    /// <response code="400">Dados inválidos fornecidos.</response>
    /// <response code="409">Conflito: Já existe uma marca com o nome informado.</response>
    [HttpPost]
    [ProducesResponseType(typeof(MarcaResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] MarcaRequestDto dto)
    {
        var traceId = HttpContext.TraceIdentifier;
        _logger.LogInformation(
            "Iniciando cadastro da marca '{Nome}' | TraceId: {TraceId}",
            dto.Nome,
            traceId);

        var criada = await _marcaService.CreateAsync(dto);

        _logger.LogInformation(
            "Marca '{MarcaId}' criada com sucesso | TraceId: {TraceId}",
            criada.Id,
            traceId);

        return CreatedAtAction(nameof(GetById), new { id = criada.Id }, criada);
    }

    /// <summary>
    /// Atualiza os dados de uma marca existente.
    /// </summary>
    /// <param name="id">Identificador único da marca.</param>
    /// <param name="dto">Novos dados da marca.</param>
    /// <returns>Marca atualizada.</returns>
    /// <response code="200">Marca atualizada com sucesso.</response>
    /// <response code="400">Dados inválidos.</response>
    /// <response code="404">Marca não encontrada.</response>
    /// <response code="409">Conflito: Já existe outra marca com esse nome.</response>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(MarcaResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(Guid id, [FromBody] MarcaRequestDto dto)
    {
        var traceId = HttpContext.TraceIdentifier;
        _logger.LogInformation(
            "Iniciando atualização da marca {MarcaId} | TraceId: {TraceId}",
            id,
            traceId);

        var atualizada = await _marcaService.UpdateAsync(id, dto);

        _logger.LogInformation(
            "Marca {MarcaId} atualizada com sucesso | TraceId: {TraceId}",
            id,
            traceId);

        return Ok(atualizada);
    }

    /// <summary>
    /// Remove uma marca do sistema.
    /// </summary>
    /// <param name="id">Identificador da marca a ser removida.</param>
    /// <returns>Sem conteúdo.</returns>
    /// <response code="204">Marca removida com sucesso.</response>
    /// <response code="404">Marca não encontrada.</response>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var traceId = HttpContext.TraceIdentifier;
        _logger.LogInformation(
            "Iniciando exclusão da marca {MarcaId} | TraceId: {TraceId}",
            id,
            traceId);

        await _marcaService.DeleteAsync(id);

        _logger.LogInformation(
            "Marca {MarcaId} excluída com sucesso | TraceId: {TraceId}",
            id,
            traceId);

        return NoContent();
    }
}
