namespace AutoHub.Domain.Entities;

public class VeiculoCliente {
    public Guid Id { get; set; }
    public string PlacaVeiculo { get; set; } = string.Empty;
    public int Ano { get; set; }
    public string? Observacoes { get; set; }
    
    //Tabelas Filhas
    public ICollection<OrdemServico> OrdemServicos { get; set; } = new List<OrdemServico>();
    
    //FKs
    public Guid ClienteId { get; set; }
    public Cliente Cliente { get; set; } = null!;
    
    public Guid ModeloId { get; set; }
    public Modelo Modelo { get; set; } = null!;
    
}