namespace AutoHub.Domain.Entities;

public class OsServico
{
    public int Quantidade  { get; set; }
    public decimal PrecoCobrado { get; set; }
    
    //Fks
    public Guid ServicoId { get; set; }
    public Servico Servico { get; set; } = null!;
    //
    public Guid OrdemServicoId { get; set; }
    public OrdemServico OrdemServico { get; set; } = null!;
}