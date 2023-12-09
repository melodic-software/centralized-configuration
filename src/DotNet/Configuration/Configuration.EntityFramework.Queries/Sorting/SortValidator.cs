using Enterprise.Core.Queries.Sorting;
using Enterprise.Events.Model.Domain.Concrete;
using Enterprise.Mapping.Properties.Services.Abstract;

namespace Configuration.EntityFramework.Queries.Sorting;

public class SortValidator<TSource, TTarget> : IValidateSort
{
    private readonly IPropertyMappingService _propertyMappingService;

    public SortValidator(IPropertyMappingService propertyMappingService)
    {
        _propertyMappingService = propertyMappingService;
    }

    public ValidationFailure? Validate(SortOptions sortOptions)
    {
        if (string.IsNullOrWhiteSpace(sortOptions.OrderBy))
            return null;

        // TODO: should this sort property be whatever the name of the input param is?
        // for example: the name of the sort parameter in a query string that is bound in an API controller method?

        return !_propertyMappingService.MappingExistsFor<TSource, TTarget>(sortOptions.OrderBy) ? 
            new ValidationFailure("Invalid sort specified", nameof(sortOptions.OrderBy)) 
            : null;
    }
}