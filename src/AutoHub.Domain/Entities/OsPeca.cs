namespace AutoHub.Domain.Entities;

public class OsPeca
{
    public int Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; }
    
    //Fks
    public Guid PecaId { get; set; }
    public Peca Peca { get; set; } = null!;
    //
    public Guid OrdemServicoId { get; set; }
    public OrdemServico OrdemServico { get; set; } = null!;
}