namespace AutoHub.Domain.Entities;

public class Marca
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;

    public ICollection<Modelo> Modelos { get; set; } = new List<Modelo>();
}