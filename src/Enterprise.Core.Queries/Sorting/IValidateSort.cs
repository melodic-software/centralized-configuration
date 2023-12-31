using Enterprise.DomainDrivenDesign.Events;

namespace Enterprise.Core.Queries.Sorting;

public interface IValidateSort
{
    ValidationFailure? Validate(SortOptions sortOptions);
}