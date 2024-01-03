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

    public IEnumerable<CompanyDto> GetByIds(IEnumerable<Guid> ids, bool trackChanges)
    {
        if (ids is null)
            throw new IdParametersBadRequestException();

        List<Guid> idList = ids.ToList();

        IEnumerable<Company> companyEntities = repository.Company.GetByIds(idList, trackChanges);

        if (idList.Count != companyEntities.Count())
            throw new CollectionByIdsBadRequestException();

        IEnumerable<CompanyDto>? companiesToReturn = mapper.Map<IEnumerable<CompanyDto>>(companyEntities);

        return companiesToReturn;
    }

    public CompanyDto CreateCompany(CompanyForCreationDto company)
    {
        Company? companyEntity = mapper.Map<Company>(company);

        repository.Company.CreateCompany(companyEntity);
        repository.Save();

        CompanyDto? companyToReturn = mapper.Map<CompanyDto>(companyEntity);

        return companyToReturn;
    }

    public (IEnumerable<CompanyDto> companies, string ids) CreateCompanyCollection(
        IEnumerable<CompanyForCreationDto> companyCollection)
    {
        if (companyCollection is null)
            throw new CompanyCollectionBadRequest();

        IEnumerable<Company>? companyEntities = mapper.Map<IEnumerable<Company>>(companyCollection);

        foreach (Company company in companyEntities)
            repository.Company.CreateCompany(company);

        repository.Save();

        IEnumerable<CompanyDto>? companyCollectionToReturn = mapper.Map<IEnumerable<CompanyDto>>(companyEntities).ToList();

        string ids = string.Join(",", companyCollectionToReturn.Select(c => c.Id));

        return (companies: companyCollectionToReturn, ids);
    }
}