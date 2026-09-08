using AutoHub.Application.DTOs.Marcas;
using AutoHub.Application.Interfaces;
using AutoHub.Domain.Entities;
using AutoHub.Domain.Exceptions;

namespace AutoHub.Application.Services;

public class MarcaService(IRepository<Marca> marcaRepository) : IMarcaService
{
    private readonly IRepository<Marca> _marcaRepository = marcaRepository;

    public async Task<IEnumerable<MarcaResponseDto>> GetAllAsync()
    {
        var marcas = await _marcaRepository.GetAllAsync();
        return marcas.Select(MapToResponseDto);
    }

    public async Task<MarcaResponseDto> GetByIdAsync(Guid id)
    {
        var marca = await _marcaRepository.GetByIdAsync(id)
            ?? throw new ResourceNotFoundException("Marca", id);

        return MapToResponseDto(marca);
    }

    public async Task<MarcaResponseDto> CreateAsync(MarcaRequestDto dto)
    {
        var marcaExistente = await _marcaRepository.FirstOrDefaultAsync(m => m.Nome.ToLower() == dto.Nome.Trim().ToLower());
        if (marcaExistente != null)
        {
            throw new ConflictException($"Já existe uma marca cadastrada com o nome '{dto.Nome}'.");
        }

        var marca = new Marca
        {
            Nome = dto.Nome.Trim()
        };

        marca.Validar();

        await _marcaRepository.AddAsync(marca);
        await _marcaRepository.SaveChangesAsync();

        return MapToResponseDto(marca);
    }

    public async Task<MarcaResponseDto> UpdateAsync(Guid id, MarcaRequestDto dto)
    {
        var marca = await _marcaRepository.GetByIdAsync(id)
            ?? throw new ResourceNotFoundException("Marca", id);

        var marcaComMesmoNome = await _marcaRepository.FirstOrDefaultAsync(m => m.Nome.ToLower() == dto.Nome.Trim().ToLower() && m.Id != id);
        if (marcaComMesmoNome != null)
        {
            throw new ConflictException($"Já existe outra marca cadastrada com o nome '{dto.Nome}'.");
        }

        marca.Nome = dto.Nome.Trim();
        marca.Validar();

        _marcaRepository.Update(marca);
        await _marcaRepository.SaveChangesAsync();

        return MapToResponseDto(marca);
    }

    public async Task DeleteAsync(Guid id)
    {
        var marca = await _marcaRepository.GetByIdAsync(id)
            ?? throw new ResourceNotFoundException("Marca", id);

        _marcaRepository.Delete(marca);
        await _marcaRepository.SaveChangesAsync();
    }

    private static MarcaResponseDto MapToResponseDto(Marca m) => new()
    {
        Id = m.Id,
        Nome = m.Nome
    };
}
