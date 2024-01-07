﻿using Enterprise.ApplicationServices.Queries.Model;

namespace Configuration.ApplicationServices.Queries.Applications.GetByUniqueName;

public class GetApplicationByUniqueName : IQuery
{
    public string UniqueName { get; }

    public GetApplicationByUniqueName(string uniqueName)
    {
        UniqueName = uniqueName;
    }
}