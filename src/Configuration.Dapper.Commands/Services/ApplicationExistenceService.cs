﻿using Configuration.Core.Domain.Services.Abstract;

namespace Configuration.Dapper.Commands.Services
{
    public class ApplicationExistenceService : IApplicationExistenceService
    {
        public Task<bool> ApplicationExistsAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ApplicationExistsAsync(string uniqueName)
        {
            throw new NotImplementedException();
        }
    }
}