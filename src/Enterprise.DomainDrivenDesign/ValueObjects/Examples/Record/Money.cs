namespace Enterprise.DomainDrivenDesign.ValueObjects.Examples.Record;

public record Money
{
    public Money(decimal Amount, Currency Currency)
    {
        this.Amount = Amount;
        this.Currency = Currency;
    }

    public static Money operator +(Money first, Money second)
    {
        EnsureCurrenciesMatch(first, second);

        decimal amount = first.Amount + second.Amount;
        Currency currency = first.Currency;

        return new Money(amount, currency);
    }

    public static Money operator -(Money first, Money second)
    {
        EnsureCurrenciesMatch(first, second);

        decimal amount = first.Amount - second.Amount;
        Currency currency = first.Currency;

        return new Money(amount, currency);
    }

    public decimal Amount { get; init; }
    public Currency Currency { get; init; }

    public static Money Zero() => new(0, Currency.None);
    public static Money Zero(Currency currency) => new(0, currency);

    public bool IsZero() => this == Zero(Currency);

    private static void EnsureCurrenciesMatch(Money first, Money second)
    {
        if (first.Currency != second.Currency)
            throw new InvalidOperationException("Currencies have to be equal.");
    }

    public void Deconstruct(out decimal Amount, out Currency Currency)
    {
        Amount = this.Amount;
        Currency = this.Currency;
    }
}