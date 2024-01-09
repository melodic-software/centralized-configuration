namespace Configuration.Domain.Applications;

public record ApplicationId
{
    public ApplicationId(Guid Value)
    {
        this.Value = Value;
    }

    public static ApplicationId New() => new(Guid.NewGuid());
    public Guid Value { get; init; }

    public void Deconstruct(out Guid Value)
    {
        Value = this.Value;
    }
}