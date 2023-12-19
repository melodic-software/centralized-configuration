using Enterprise.Events.Model.Domain.Concrete;

namespace Enterprise.Core.Queries.Sorting;

public interface IValidateSort
{
    ValidationFailure? Validate(SortOptions sortOptions);
}