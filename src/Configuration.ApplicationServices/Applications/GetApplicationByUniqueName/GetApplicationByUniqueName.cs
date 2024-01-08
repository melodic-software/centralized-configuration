using Enterprise.ApplicationServices.Queries.Model;

namespace Configuration.ApplicationServices.Applications.GetApplicationByUniqueName;

public class GetApplicationByUniqueName : IQuery
{
    public string UniqueName { get; }

    public GetApplicationByUniqueName(string uniqueName)
    {
        UniqueName = uniqueName;
    }
}