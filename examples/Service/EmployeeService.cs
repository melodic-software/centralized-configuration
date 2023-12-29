using AutoMapper;
using Contracts;
using Service.Contracts;

namespace Service
{
    internal sealed class EmployeeService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper) : IEmployeeService
    {

    }
}
