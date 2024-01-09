namespace Enterprise.DomainDrivenDesign.ValueObjects.Examples.Record;

// Records have been available since C# 9 and .NET 5
// They natively support all the immutability and equality functionality that previously would have been implemented in a base class.

public record Address
{
    public Address(string Street,
        string City,
        string StateProvince,
        string ZipCode,
        string Country)
    {
        this.Street = Street;
        this.City = City;
        this.StateProvince = StateProvince;
        this.ZipCode = ZipCode;
        this.Country = Country;
    }

    public string Street { get; init; }
    public string City { get; init; }
    public string StateProvince { get; init; }
    public string ZipCode { get; init; }
    public string Country { get; init; }

    public void Deconstruct(out string Street, out string City, out string StateProvince, out string ZipCode, out string Country)
    {
        Street = this.Street;
        City = this.City;
        StateProvince = this.StateProvince;
        ZipCode = this.ZipCode;
        Country = this.Country;
    }
}