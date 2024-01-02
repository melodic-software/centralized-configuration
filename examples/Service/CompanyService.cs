using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace Service;

internal sealed class CompanyService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper) : ICompanyService
{
    public IEnumerable<CompanyDto> GetAllCompanies(bool trackChanges)
    {
        IEnumerable<Company> companies = repository.Company.GetAllCompanies(trackChanges);
        IEnumerable<CompanyDto>? companiesDto = mapper.Map<IEnumerable<CompanyDto>>(companies);
        return companiesDto;
    }

    public CompanyDto GetCompany(Guid companyId, bool trackChanges)
    {
        Company? company = repository.Company.GetCompany(companyId, trackChanges);

        if (company == null)
            throw new CompanyNotFoundException(companyId);

        CompanyDto? companyDto = mapper.Map<CompanyDto>(company);
            
        return companyDto;
    }
}