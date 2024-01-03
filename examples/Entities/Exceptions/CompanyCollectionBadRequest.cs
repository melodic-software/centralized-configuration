using Enterprise.Exceptions;

namespace Entities.Exceptions
{
    public sealed class CompanyCollectionBadRequest()
        : BadRequestException("Company collection sent from a client is null.");
}
