﻿namespace Configuration.API.Client.Models.Input.V1;

public class GetApplicationsModel
{
    public string? Name { get; set; }
    public string? AbbreviatedName { get; set; }
    public bool? IsActive { get; set; }
    public string? SearchQuery { get; set; }
    public int? PageNumber { get; set; }
    public int? PageSize { get; set; }
    public string? OrderBy { get; set; }
    public string? Properties { get; set; }
}