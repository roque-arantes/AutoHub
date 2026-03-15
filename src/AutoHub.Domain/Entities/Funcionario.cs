namespace AutoHub.Domain.Entities;

public class Funcionario
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Matricula { get; set; } = string.Empty;
    public string Cargo { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public string? Email { get; set; }
    public DateTime DataAdmissao { get; set; }
    public decimal Salario { get; set; }
    public string Status { get; set; } = string.Empty;
    
    
    // Tabelas Filhas
    public ICollection<OrdemServico> OrdemServicos { get; set; } = new List<OrdemServico>();
    public ICollection<Venda> Vendas { get; set; } = new List<Venda>();
}