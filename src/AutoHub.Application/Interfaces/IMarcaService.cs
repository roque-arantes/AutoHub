using AutoHub.Application.DTOs.Marcas;

namespace AutoHub.Application.Interfaces;

public interface IMarcaService
{
    Task<IEnumerable<MarcaResponseDto>> GetAllAsync();
    Task<MarcaResponseDto> GetByIdAsync(Guid id);
    Task<MarcaResponseDto> CreateAsync(MarcaRequestDto dto);
    Task<MarcaResponseDto> UpdateAsync(Guid id, MarcaRequestDto dto);
    Task DeleteAsync(Guid id);
}
