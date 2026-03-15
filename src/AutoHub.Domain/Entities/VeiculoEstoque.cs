namespace AutoHub.Domain.Entities;

public class VeiculoEstoque
{
    public Guid Id { get; set; }
    public int AnoFabricacao { get; set; }
    public string Cor { get; set; } = string.Empty;
    public int Kilometragem { get; set; }
    public decimal Preco { get; set; }
    public string Status { get; set; } = string.Empty;
    public string Chassis { get; set; } = string.Empty;
    
    //Tabelas Filhas
    public Venda? Venda { get; set; }
    
    //FKs
    public Guid ModeloId { get; set; }
    public Modelo Modelo { get; set; } = null!;

}