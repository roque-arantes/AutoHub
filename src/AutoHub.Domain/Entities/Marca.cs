using AutoHub.Domain.Common;
using AutoHub.Domain.Exceptions;

namespace AutoHub.Domain.Entities;

public class Marca : BaseEntity
{
    public string Nome { get; set; } = string.Empty;

    public ICollection<Modelo> Modelos { get; set; } = new List<Modelo>();

    public void Validar()
    {
        if (string.IsNullOrWhiteSpace(Nome))
            throw new DomainException("O nome da marca é obrigatório.");

        if (Nome.Trim().Length < 2)
            throw new DomainException("O nome da marca deve ter pelo menos 2 caracteres.");
    }
}