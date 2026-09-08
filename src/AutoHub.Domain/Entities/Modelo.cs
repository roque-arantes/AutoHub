using AutoHub.Domain.Common;

namespace AutoHub.Domain.Entities;

public class Modelo : BaseEntity
{
    public string Nome { get; set; } = string.Empty;
    
    // FK
    public Guid MarcaId { get; set; }
    // Navigation property
    public Marca Marca { get; set; } = null!;
    
    public ICollection<VeiculoCliente> VeiculosCliente { get; set; } = new List<VeiculoCliente>();
    public ICollection<VeiculoEstoque> VeiculosEstoque { get; set; } = new List<VeiculoEstoque>();
}