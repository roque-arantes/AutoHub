using AutoHub.Application.DTOs.Clientes;

namespace AutoHub.Application.Interfaces;

public interface IClienteService
{
    Task<IEnumerable<ClienteResponseDto>> GetAllAsync();
    Task<ClienteResponseDto> GetByIdAsync(Guid id);
    Task<ClienteResponseDto> CreateAsync(ClienteRequestDto dto);
    Task<ClienteResponseDto> UpdateAsync(Guid id, ClienteRequestDto dto);
    Task DeleteAsync(Guid id);
}
