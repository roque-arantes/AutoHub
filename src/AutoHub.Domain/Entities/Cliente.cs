using AutoHub.Domain.Common;
using AutoHub.Domain.Exceptions;

namespace AutoHub.Domain.Entities;

public class Cliente : BaseEntity
{
    public string Nome { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string Logradouro { get; set; } = string.Empty;
    public string Numero { get; set; } = string.Empty;
    public string Cidade { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public string Cep { get; set; } = string.Empty;

    // Tabelas Filhas
    public ICollection<VeiculoCliente> VeiculosCliente { get; set; } = new List<VeiculoCliente>();
    public ICollection<Venda> Vendas { get; set; } = new List<Venda>();

    public void Validar()
    {
        if (string.IsNullOrWhiteSpace(Nome))
            throw new DomainException("O nome do cliente é obrigatório.");

        if (string.IsNullOrWhiteSpace(Cpf))
            throw new DomainException("O CPF do cliente é obrigatório.");

        var apenasDigitosCpf = new string(Cpf.Where(char.IsDigit).ToArray());
        if (apenasDigitosCpf.Length != 11)
            throw new DomainException("O CPF deve conter exatamente 11 dígitos numéricos.");

        if (!string.IsNullOrWhiteSpace(Email) && !Email.Contains('@'))
            throw new DomainException("O formato do e-mail informado é inválido.");
    }
}