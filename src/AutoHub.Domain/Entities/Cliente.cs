namespace AutoHub.Domain.Entities;

public class Cliente
{
    public Guid Id { get; set; } 
    public string Nome { get; set; }  = string.Empty;
    public string Cpf { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string Logradouro { get; set; }  = string.Empty;
    public string Numero { get; set; } = string.Empty;
    public string Cidade { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public string Cep { get; set; }  = string.Empty;
    
    
    //Tabelas Filhas
    public ICollection<VeiculoCliente> VeiculosCliente { get; set; } = new List<VeiculoCliente>();
    public ICollection<Venda> Vendas { get; set; } = new List<Venda>();
}