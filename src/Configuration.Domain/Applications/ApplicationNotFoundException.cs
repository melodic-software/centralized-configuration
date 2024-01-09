using Enterprise.Exceptions;

namespace Configuration.Domain.Applications;

public class ApplicationNotFoundException : NotFoundException
{
    public ApplicationNotFoundException(string message) : base(message)
    {
    }
}