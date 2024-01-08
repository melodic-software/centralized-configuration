namespace Configuration.ApplicationServices.Applications.GetApplications;

public class ApplicationFilterOptions
{
    public string? Name { get; set; }
    public string? AbbreviatedName { get; set; }
    public bool? IsActive { get; set; }

    public ApplicationFilterOptions(string? name, string? abbreviatedName, bool? isActive)
    {
        Name = name?.Trim();
        AbbreviatedName = abbreviatedName?.Trim();
        IsActive = isActive;
    }
}