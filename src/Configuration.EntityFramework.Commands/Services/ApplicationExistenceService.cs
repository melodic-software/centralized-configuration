using Configuration.Domain.Applications;
using Configuration.EntityFramework.DbContexts.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Configuration.EntityFramework.Commands.Services;

public class ApplicationExistenceService : IApplicationExistenceService
{
    private readonly ConfigurationContext _context;

    public ApplicationExistenceService(ConfigurationContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<bool> ApplicationExistsAsync(Guid id)
    {
        return await _context.Applications.AnyAsync(c => c.DomainId == id);
    }

    public async Task<bool> ApplicationExistsAsync(string uniqueName)
    {
        return await _context.Applications.AnyAsync(c => c.UniqueName == uniqueName);
    }
}