using AutoHub.Application.DTOs;
using AutoHub.Application.DTOs.VeiculosEstoque;
using AutoHub.Application.Interfaces;
using AutoHub.Domain.Entities;
using AutoHub.Domain.Exceptions;

namespace AutoHub.Application.Services;

public class VeiculoEstoqueService(
    IRepository<VeiculoEstoque> veiculoRepository,
    IRepository<Modelo> modeloRepository) : IVeiculoEstoqueService
{
    private readonly IRepository<VeiculoEstoque> _veiculoRepository = veiculoRepository;
    private readonly IRepository<Modelo> _modeloRepository = modeloRepository;

    public async Task<IEnumerable<VeiculoEstoqueResponseDto>> GetAllAsync()
    {
        var veiculos = await _veiculoRepository.GetAllAsync();
        return veiculos.Select(MapToResponseDto);
    }

    public async Task<PagedResult<VeiculoEstoqueResponseDto>> GetPagedAsync(int page, int pageSize)
    {
        if (page < 1)
            throw new ArgumentException("O parâmetro 'page' deve ser maior ou igual a 1.");

        if (pageSize < 1 || pageSize > 100)
            throw new ArgumentException("O parâmetro 'pageSize' deve estar entre 1 e 100.");

        var (items, totalCount) = await _veiculoRepository.GetPagedAsync(page, pageSize);

        return new PagedResult<VeiculoEstoqueResponseDto>
        {
            Page = page,
            PageSize = pageSize,
            TotalItems = totalCount,
            TotalPages = (int)Math.Ceiling((double)totalCount / pageSize),
            Items = items.Select(MapToResponseDto)
        };
    }

    public async Task<VeiculoEstoqueResponseDto> GetByIdAsync(Guid id)
    {
        var veiculo = await _veiculoRepository.GetByIdAsync(id)
            ?? throw new ResourceNotFoundException("VeiculoEstoque", id);

        return MapToResponseDto(veiculo);
    }

    public async Task<VeiculoEstoqueResponseDto> CreateAsync(VeiculoEstoqueRequestDto dto)
    {
        var modeloExiste = await _modeloRepository.ExistsByIdAsync(dto.ModeloId);
        if (!modeloExiste)
        {
            throw new ResourceNotFoundException("Modelo", dto.ModeloId);
        }

        var chassisLimpo = dto.Chassis.Trim().ToUpperInvariant();
        var chassiExistente = await _veiculoRepository.FirstOrDefaultAsync(v => v.Chassis.ToUpper() == chassisLimpo);
        if (chassiExistente != null)
        {
            throw new ConflictException($"Já existe um veículo cadastrado com o chassi '{dto.Chassis}'.");
        }

        var veiculo = new VeiculoEstoque
        {
            AnoFabricacao = dto.AnoFabricacao,
            Cor = dto.Cor.Trim(),
            Kilometragem = dto.Kilometragem,
            Preco = dto.Preco,
            Status = dto.Status.Trim(),
            Chassis = chassisLimpo,
            ModeloId = dto.ModeloId
        };

        veiculo.Validar();

        await _veiculoRepository.AddAsync(veiculo);
        await _veiculoRepository.SaveChangesAsync();

        return MapToResponseDto(veiculo);
    }

    public async Task<VeiculoEstoqueResponseDto> UpdateAsync(Guid id, VeiculoEstoqueRequestDto dto)
    {
        var veiculo = await _veiculoRepository.GetByIdAsync(id)
            ?? throw new ResourceNotFoundException("VeiculoEstoque", id);

        var modeloExiste = await _modeloRepository.ExistsByIdAsync(dto.ModeloId);
        if (!modeloExiste)
        {
            throw new ResourceNotFoundException("Modelo", dto.ModeloId);
        }

        var chassisLimpo = dto.Chassis.Trim().ToUpperInvariant();
        var chassiExistente = await _veiculoRepository.FirstOrDefaultAsync(v => v.Chassis.ToUpper() == chassisLimpo && v.Id != id);
        if (chassiExistente != null)
        {
            throw new ConflictException($"Já existe outro veículo cadastrado com o chassi '{dto.Chassis}'.");
        }

        veiculo.AnoFabricacao = dto.AnoFabricacao;
        veiculo.Cor = dto.Cor.Trim();
        veiculo.Kilometragem = dto.Kilometragem;
        veiculo.Preco = dto.Preco;
        veiculo.Status = dto.Status.Trim();
        veiculo.Chassis = chassisLimpo;
        veiculo.ModeloId = dto.ModeloId;

        veiculo.Validar();

        _veiculoRepository.Update(veiculo);
        await _veiculoRepository.SaveChangesAsync();

        return MapToResponseDto(veiculo);
    }

    public async Task DeleteAsync(Guid id)
    {
        var veiculo = await _veiculoRepository.GetByIdAsync(id)
            ?? throw new ResourceNotFoundException("VeiculoEstoque", id);

        _veiculoRepository.Delete(veiculo);
        await _veiculoRepository.SaveChangesAsync();
    }

    private static VeiculoEstoqueResponseDto MapToResponseDto(VeiculoEstoque v) => new()
    {
        Id = v.Id,
        AnoFabricacao = v.AnoFabricacao,
        Cor = v.Cor,
        Kilometragem = v.Kilometragem,
        Preco = v.Preco,
        Status = v.Status,
        Chassis = v.Chassis,
        ModeloId = v.ModeloId
    };
}
