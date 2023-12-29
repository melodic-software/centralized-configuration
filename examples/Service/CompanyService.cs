using AutoMapper;
using Contracts;
using Entities.Models;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace Service
{
    internal sealed class CompanyService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper) : ICompanyService
    {
        public IEnumerable<CompanyDto> GetAllCompanies(bool trackChanges)
        {
            IEnumerable<Company> companies = repository.Company.GetAllCompanies(trackChanges);
            IEnumerable<CompanyDto>? companiesDto = mapper.Map<IEnumerable<CompanyDto>>(companies);
            return companiesDto;
        }
    }
}