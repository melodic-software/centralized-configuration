﻿using Contracts;
using Entities.Models;

namespace Repository;

public class CompanyRepository(RepositoryContext repositoryContext)
    : RepositoryBase<Company>(repositoryContext), ICompanyRepository
{
    public IEnumerable<Company> GetAllCompanies(bool trackChanges) =>
        FindAll(trackChanges)
            .OrderBy(c => c.Name)
            .ToList();

    public Company? GetCompany(Guid companyId, bool trackChanges) =>
        FindByCondition(c => c.Id.Equals(companyId), trackChanges)
            .SingleOrDefault();
}