using System.ComponentModel.DataAnnotations;

namespace AutoHub.Application.DTOs.Clientes;

public class ClienteRequestDto
{
    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "O nome deve ter entre 2 e 100 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "O CPF é obrigatório.")]
    [RegularExpression(@"^\d{11}$|^\d{3}\.\d{3}\.\d{3}-\d{2}$", ErrorMessage = "O CPF deve ter 11 dígitos ou estar no formato 000.000.000-00.")]
    public string Cpf { get; set; } = string.Empty;

    [Required(ErrorMessage = "O telefone é obrigatório.")]
    public string Telefone { get; set; } = string.Empty;

    [EmailAddress(ErrorMessage = "E-mail em formato inválido.")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "O logradouro é obrigatório.")]
    public string Logradouro { get; set; } = string.Empty;

    [Required(ErrorMessage = "O número é obrigatório.")]
    public string Numero { get; set; } = string.Empty;

    [Required(ErrorMessage = "A cidade é obrigatória.")]
    public string Cidade { get; set; } = string.Empty;

    [Required(ErrorMessage = "O estado é obrigatório.")]
    [StringLength(2, MinimumLength = 2, ErrorMessage = "O estado deve ter 2 caracteres (UF).")]
    public string Estado { get; set; } = string.Empty;

    [Required(ErrorMessage = "O CEP é obrigatório.")]
    public string Cep { get; set; } = string.Empty;
}
