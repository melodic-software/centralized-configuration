namespace Shared.DataTransferObjects;

[Serializable]
public record EmployeeDto(Guid Id, string Name, int Age, string Position); // This is a positional record.