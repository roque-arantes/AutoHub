namespace AutoHub.Application.DTOs.VeiculosEstoque;

public class VeiculoEstoqueResponseDto
{
    public Guid Id { get; set; }
    public int AnoFabricacao { get; set; }
    public string Cor { get; set; } = string.Empty;
    public int Kilometragem { get; set; }
    public decimal Preco { get; set; }
    public string Status { get; set; } = string.Empty;
    public string Chassis { get; set; } = string.Empty;
    public Guid ModeloId { get; set; }
}
