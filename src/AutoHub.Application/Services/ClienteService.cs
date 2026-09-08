using AutoHub.Application.DTOs.Clientes;
using AutoHub.Application.Interfaces;
using AutoHub.Domain.Entities;
using AutoHub.Domain.Exceptions;

namespace AutoHub.Application.Services;

public class ClienteService(IRepository<Cliente> clienteRepository) : IClienteService
{
    private readonly IRepository<Cliente> _clienteRepository = clienteRepository;

    public async Task<IEnumerable<ClienteResponseDto>> GetAllAsync()
    {
        var clientes = await _clienteRepository.GetAllAsync();
        return clientes.Select(MapToResponseDto);
    }

    public async Task<ClienteResponseDto> GetByIdAsync(Guid id)
    {
        var cliente = await _clienteRepository.GetByIdAsync(id)
            ?? throw new ResourceNotFoundException("Cliente", id);

        return MapToResponseDto(cliente);
    }

    public async Task<ClienteResponseDto> CreateAsync(ClienteRequestDto dto)
    {
        var cpfLimpo = new string(dto.Cpf.Where(char.IsDigit).ToArray());

        var clienteExistente = await _clienteRepository.FirstOrDefaultAsync(c => c.Cpf == cpfLimpo || c.Cpf == dto.Cpf);
        if (clienteExistente != null)
        {
            throw new ConflictException($"Já existe um cliente cadastrado com o CPF '{dto.Cpf}'.");
        }

        var cliente = new Cliente
        {
            Nome = dto.Nome,
            Cpf = cpfLimpo,
            Telefone = dto.Telefone,
            Email = dto.Email,
            Logradouro = dto.Logradouro,
            Numero = dto.Numero,
            Cidade = dto.Cidade,
            Estado = dto.Estado.ToUpperInvariant(),
            Cep = dto.Cep
        };

        cliente.Validar();

        await _clienteRepository.AddAsync(cliente);
        await _clienteRepository.SaveChangesAsync();

        return MapToResponseDto(cliente);
    }

    public async Task<ClienteResponseDto> UpdateAsync(Guid id, ClienteRequestDto dto)
    {
        var cliente = await _clienteRepository.GetByIdAsync(id)
            ?? throw new ResourceNotFoundException("Cliente", id);

        var cpfLimpo = new string(dto.Cpf.Where(char.IsDigit).ToArray());

        var clienteComMesmoCpf = await _clienteRepository.FirstOrDefaultAsync(c => (c.Cpf == cpfLimpo || c.Cpf == dto.Cpf) && c.Id != id);
        if (clienteComMesmoCpf != null)
        {
            throw new ConflictException($"Já existe outro cliente cadastrado com o CPF '{dto.Cpf}'.");
        }

        cliente.Nome = dto.Nome;
        cliente.Cpf = cpfLimpo;
        cliente.Telefone = dto.Telefone;
        cliente.Email = dto.Email;
        cliente.Logradouro = dto.Logradouro;
        cliente.Numero = dto.Numero;
        cliente.Cidade = dto.Cidade;
        cliente.Estado = dto.Estado.ToUpperInvariant();
        cliente.Cep = dto.Cep;

        cliente.Validar();

        _clienteRepository.Update(cliente);
        await _clienteRepository.SaveChangesAsync();

        return MapToResponseDto(cliente);
    }

    public async Task DeleteAsync(Guid id)
    {
        var cliente = await _clienteRepository.GetByIdAsync(id)
            ?? throw new ResourceNotFoundException("Cliente", id);

        _clienteRepository.Delete(cliente);
        await _clienteRepository.SaveChangesAsync();
    }

    private static ClienteResponseDto MapToResponseDto(Cliente c) => new()
    {
        Id = c.Id,
        Nome = c.Nome,
        Cpf = c.Cpf,
        Telefone = c.Telefone,
        Email = c.Email,
        Logradouro = c.Logradouro,
        Numero = c.Numero,
        Cidade = c.Cidade,
        Estado = c.Estado,
        Cep = c.Cep
    };
}
