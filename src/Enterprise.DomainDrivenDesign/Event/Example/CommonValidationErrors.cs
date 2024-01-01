namespace Enterprise.DomainDrivenDesign.Event.Example;

public static class CommonValidationErrors
{
    public static ValidationFailure NotFound = new(
        "Entity.NotFound",
        "The entity with the specified identifier was not found");
}