namespace Enterprise.DomainDrivenDesign.ValueObjects.Examples.Record;

public record Email
{
    public Email(string Value)
    {
        this.Value = Value;
    }

    public string Value { get; init; }

    public void Deconstruct(out string Value)
    {
        Value = this.Value;
    }
}