using Enterprise.DomainDrivenDesign.Event;

namespace Enterprise.Core.Queries.Sorting;

public interface IValidateSort
{
    ValidationFailure? Validate(SortOptions sortOptions);
}