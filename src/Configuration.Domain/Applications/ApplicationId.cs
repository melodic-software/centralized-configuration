namespace Configuration.Domain.Applications;

public record ApplicationId(Guid Value)
{
    public static ApplicationId New() => new(Guid.NewGuid());
}