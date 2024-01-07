using Configuration.Domain.Applications;
using Configuration.EntityFramework.DbContexts.Configuration;
using Microsoft.EntityFrameworkCore;
using ApplicationId = Configuration.Domain.Applications.ApplicationId;

namespace Configuration.EntityFramework.Commands.Services;

public class ApplicationExistenceService : IApplicationExistenceService
{
    private readonly ConfigurationContext _context;

    public ApplicationExistenceService(ConfigurationContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<bool> ApplicationExistsAsync(ApplicationId id)
    {
        return await _context.Applications.AnyAsync(c => c.DomainId == id.Value);
    }

    public async Task<bool> ApplicationExistsAsync(string uniqueName)
    {
        return await _context.Applications.AnyAsync(c => c.UniqueName == uniqueName);
    }
}