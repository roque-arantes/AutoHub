using AutoHub.Domain.Common;
using AutoHub.Domain.Exceptions;

namespace AutoHub.Domain.Entities;

public class VeiculoEstoque : BaseEntity
{
    public int AnoFabricacao { get; set; }
    public string Cor { get; set; } = string.Empty;
    public int Kilometragem { get; set; }
    public decimal Preco { get; set; }
    public string Status { get; set; } = "Disponivel";
    public string Chassis { get; set; } = string.Empty;

    // Tabelas Filhas
    public Venda? Venda { get; set; }

    // FKs
    public Guid ModeloId { get; set; }
    public Modelo Modelo { get; set; } = null!;

    public void Validar()
    {
        if (AnoFabricacao < 1900 || AnoFabricacao > DateTime.UtcNow.Year + 1)
            throw new DomainException($"Ano de fabricação deve estar entre 1900 e {DateTime.UtcNow.Year + 1}.");

        if (Preco <= 0)
            throw new DomainException("O preço do veículo deve ser maior que zero.");

        if (Kilometragem < 0)
            throw new DomainException("A quilometragem não pode ser negativa.");

        if (string.IsNullOrWhiteSpace(Chassis) || Chassis.Trim().Length < 17)
            throw new DomainException("O chassi deve possuir no mínimo 17 caracteres.");
    }
}