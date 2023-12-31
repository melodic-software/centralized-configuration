namespace Enterprise.Domain.Validation
{
    public class CommonErrors
    {
        public static Error EntityNotFound = new(
            "Entity.NotFound",
            "The entity with the specified identifier was not found.");
    }
}
