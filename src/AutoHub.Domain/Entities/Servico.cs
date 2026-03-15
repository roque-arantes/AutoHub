namespace AutoHub.Domain.Entities;

public class Servico
{
    public Guid Id { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public decimal PrecoBase { get; set; }
    
    //Tabelas Filhas
    public ICollection<OsServico> OsServicos { get; set; } = new List<OsServico>();
    
}