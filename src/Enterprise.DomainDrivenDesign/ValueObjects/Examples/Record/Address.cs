namespace Enterprise.DomainDrivenDesign.ValueObjects.Examples.Record;

// Records have been available since C# 9 and .NET 5
// They natively support all the immutability and equality functionality that previously would have been implemented in a base class.

public record Address(
    string Street,
    string City,
    string StateProvince,
    string ZipCode,
    string Country
);