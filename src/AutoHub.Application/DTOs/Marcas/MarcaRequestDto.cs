using System.ComponentModel.DataAnnotations;

namespace AutoHub.Application.DTOs.Marcas;

public class MarcaRequestDto
{
    [Required(ErrorMessage = "O nome da marca é obrigatório.")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "O nome da marca deve ter entre 2 e 50 caracteres.")]
    public string Nome { get; set; } = string.Empty;
}
