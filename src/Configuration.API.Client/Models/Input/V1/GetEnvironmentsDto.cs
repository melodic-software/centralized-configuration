namespace Configuration.API.Client.Models.Input.V1;

public class GetEnvironmentsDto
{
    public Guid? EnvironmentId { get; set; }
    public string? EnvironmentUniqueName { get; set; }
}