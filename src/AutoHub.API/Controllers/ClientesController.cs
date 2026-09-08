using AutoHub.Application.DTOs.Clientes;
using AutoHub.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AutoHub.API.Controllers;

/// <summary>
/// Gerenciamento de clientes da concessionária e oficina mecânica.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ClientesController(
    IClienteService clienteService,
    ILogger<ClientesController> logger) : ControllerBase
{
    private readonly IClienteService _clienteService = clienteService;
    private readonly ILogger<ClientesController> _logger = logger;

    /// <summary>
    /// Lista todos os clientes cadastrados.
    /// </summary>
    /// <returns>Lista de clientes.</returns>
    /// <response code="200">Clientes retornados com sucesso.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ClienteResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var clientes = await _clienteService.GetAllAsync();
        return Ok(clientes);
    }

    /// <summary>
    /// Obtém os dados de um cliente pelo seu identificador único (ID).
    /// </summary>
    /// <param name="id">Identificador do cliente (GUID).</param>
    /// <returns>Dados do cliente encontrado.</returns>
    /// <response code="200">Cliente retornado com sucesso.</response>
    /// <response code="404">Cliente não encontrado.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ClienteResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var cliente = await _clienteService.GetByIdAsync(id);
        return Ok(cliente);
    }

    /// <summary>
    /// Cadastra um novo cliente no sistema.
    /// </summary>
    /// <param name="dto">Dados do cliente para criação.</param>
    /// <returns>Cliente cadastrado com seu respectivo ID.</returns>
    /// <response code="201">Cliente cadastrado com sucesso.</response>
    /// <response code="400">Dados inválidos fornecidos no corpo da requisição.</response>
    /// <response code="409">Conflito: Já existe um cliente com o CPF informado.</response>
    [HttpPost]
    [ProducesResponseType(typeof(ClienteResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] ClienteRequestDto dto)
    {
        var traceId = HttpContext.TraceIdentifier;
        _logger.LogInformation(
            "Iniciando cadastro de cliente com CPF {Cpf} | TraceId: {TraceId}",
            dto.Cpf,
            traceId);

        var criado = await _clienteService.CreateAsync(dto);

        _logger.LogInformation(
            "Cliente {ClienteId} cadastrado com sucesso | TraceId: {TraceId}",
            criado.Id,
            traceId);

        return CreatedAtAction(nameof(GetById), new { id = criado.Id }, criado);
    }

    /// <summary>
    /// Atualiza os dados de um cliente existente.
    /// </summary>
    /// <param name="id">Identificador único do cliente.</param>
    /// <param name="dto">Dados atualizados do cliente.</param>
    /// <returns>Cliente com os dados atualizados.</returns>
    /// <response code="200">Cliente atualizado com sucesso.</response>
    /// <response code="400">Dados inválidos fornecidos na requisição.</response>
    /// <response code="404">Cliente não encontrado.</response>
    /// <response code="409">Conflito: Outro cliente já utiliza o CPF informado.</response>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ClienteResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(Guid id, [FromBody] ClienteRequestDto dto)
    {
        var traceId = HttpContext.TraceIdentifier;
        _logger.LogInformation(
            "Iniciando atualização do cliente {ClienteId} | TraceId: {TraceId}",
            id,
            traceId);

        var atualizado = await _clienteService.UpdateAsync(id, dto);

        _logger.LogInformation(
            "Cliente {ClienteId} atualizado com sucesso | TraceId: {TraceId}",
            id,
            traceId);

        return Ok(atualizado);
    }

    /// <summary>
    /// Remove um cliente do sistema.
    /// </summary>
    /// <param name="id">Identificador único do cliente.</param>
    /// <returns>Sem conteúdo.</returns>
    /// <response code="204">Cliente removido com sucesso.</response>
    /// <response code="404">Cliente não encontrado.</response>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var traceId = HttpContext.TraceIdentifier;
        _logger.LogInformation(
            "Iniciando remoção do cliente {ClienteId} | TraceId: {TraceId}",
            id,
            traceId);

        await _clienteService.DeleteAsync(id);

        _logger.LogInformation(
            "Cliente {ClienteId} removido com sucesso | TraceId: {TraceId}",
            id,
            traceId);

        return NoContent();
    }
}
