namespace Configuration.API.Client.DTOs.Input.V1;

public class GetEnvironmentsDto
{
    public Guid? EnvironmentId { get; set; }
    public string? EnvironmentUniqueName { get; set; }
}