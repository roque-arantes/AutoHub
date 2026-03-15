namespace AutoHub.Domain.Entities;

public class Venda
{
    public Guid Id { get; set; }
    public DateTime DataVenda  { get; set; }
    public decimal PrecoOriginal { get; set; }
    public decimal? Desconto  { get; set; }
    public decimal ValorTotal { get; set; }
    public string FormaPagamento { get; set; } = string.Empty;
    
    //FKs
    public Guid FuncionarioId { get; set; }
    public Funcionario Funcionario { get; set; } = null!;
    //
    public Guid VeiculoEstoqueId { get; set; }
    public VeiculoEstoque VeiculoEstoque { get; set; } = null!;
    //
    public Guid ClienteId { get; set; }
    public Cliente Cliente { get; set; } = null!;
}