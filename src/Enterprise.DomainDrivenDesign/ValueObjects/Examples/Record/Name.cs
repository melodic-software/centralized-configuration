namespace Enterprise.DomainDrivenDesign.ValueObjects.Examples.Record;

public record Name
{
    public Name(string Value)
    {
        this.Value = Value;
    }

    public string Value { get; init; }

    public void Deconstruct(out string Value)
    {
        Value = this.Value;
    }
}