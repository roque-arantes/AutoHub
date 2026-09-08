namespace AutoHub.Domain.Exceptions;

public class ResourceNotFoundException : DomainException
{
    public ResourceNotFoundException(string resourceName, object key)
        : base($"{resourceName} com identificador '{key}' não foi encontrado(a).")
    {
    }

    public ResourceNotFoundException(string message) : base(message)
    {
    }
}
