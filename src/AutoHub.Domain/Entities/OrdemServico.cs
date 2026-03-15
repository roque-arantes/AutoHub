namespace AutoHub.Domain.Entities;

public class OrdemServico
{
    public Guid Id { get; set; }
    public DateTime DataAbertura { get; set; }
    public DateTime? DataConclusao { get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal? ValorFinal { get; set; }
    public string? Diagnostico  { get; set; }
    
    //Tabelas Filhas
    public ICollection<OsServico> OsServicos { get; set; } = new List<OsServico>();
    public ICollection<OsPeca> OsPecas { get; set; } = new List<OsPeca>();
    
    //FKs
    public Guid FuncionarioId { get; set; }
    public Funcionario Funcionario { get; set; } = null!;
    //
    public Guid VeiculoClienteId { get; set; }
    public VeiculoCliente VeiculoCliente { get; set; } = null!;
    //
    
}