using Enterprise.Exceptions;

namespace Configuration.Domain.Applications;

public class ApplicationNotFoundException(string message) : NotFoundException(message);