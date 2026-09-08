using System.ComponentModel.DataAnnotations;

namespace AutoHub.Application.DTOs.VeiculosEstoque;

public class VeiculoEstoqueRequestDto
{
    [Required(ErrorMessage = "O ano de fabricação é obrigatório.")]
    [Range(1900, 2100, ErrorMessage = "O ano de fabricação deve ser válido.")]
    public int AnoFabricacao { get; set; }

    [Required(ErrorMessage = "A cor é obrigatória.")]
    public string Cor { get; set; } = string.Empty;

    [Range(0, 1000000, ErrorMessage = "A quilometragem deve ser maior ou igual a zero.")]
    public int Kilometragem { get; set; }

    [Required(ErrorMessage = "O preço é obrigatório.")]
    [Range(0.01, 10000000.0, ErrorMessage = "O preço deve ser maior que zero.")]
    public decimal Preco { get; set; }

    [Required(ErrorMessage = "O status é obrigatório.")]
    public string Status { get; set; } = "Disponivel";

    [Required(ErrorMessage = "O chassi é obrigatório.")]
    [StringLength(30, MinimumLength = 17, ErrorMessage = "O chassi deve ter no mínimo 17 caracteres.")]
    public string Chassis { get; set; } = string.Empty;

    [Required(ErrorMessage = "O modelo é obrigatório.")]
    public Guid ModeloId { get; set; }
}
