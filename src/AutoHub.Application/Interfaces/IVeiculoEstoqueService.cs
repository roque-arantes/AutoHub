using AutoHub.Application.DTOs;
using AutoHub.Application.DTOs.VeiculosEstoque;

namespace AutoHub.Application.Interfaces;

public interface IVeiculoEstoqueService
{
    Task<IEnumerable<VeiculoEstoqueResponseDto>> GetAllAsync();
    Task<PagedResult<VeiculoEstoqueResponseDto>> GetPagedAsync(int page, int pageSize);
    Task<VeiculoEstoqueResponseDto> GetByIdAsync(Guid id);
    Task<VeiculoEstoqueResponseDto> CreateAsync(VeiculoEstoqueRequestDto dto);
    Task<VeiculoEstoqueResponseDto> UpdateAsync(Guid id, VeiculoEstoqueRequestDto dto);
    Task DeleteAsync(Guid id);
}
