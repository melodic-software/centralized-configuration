namespace Enterprise.DomainDrivenDesign.ValueObject.Examples;

public record Money(decimal Amount, Currency Currency)
{
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

    public static Money Zero() => new(0, Currency.None);

    private static void EnsureCurrenciesMatch(Money first, Money second)
    {
        if (first.Currency != second.Currency)
            throw new InvalidOperationException("Currencies have to be equal.");
    }
}