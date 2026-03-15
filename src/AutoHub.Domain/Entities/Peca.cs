namespace AutoHub.Domain.Entities;

public class Peca
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string CodigoFabricante { get; set; } = string.Empty;
    public decimal PrecoUnitario { get; set; }
    public int EstoqueQuantidade { get; set; }
    
    //Tabelas Filhas
    public ICollection<OsPeca> OsPecas { get; set; } = new List<OsPeca>();
    
    
}