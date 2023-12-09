using Enterprise.Mapping.Properties.Model;

namespace Enterprise.Mapping.Properties.Services.Abstract;

public interface IPropertyMappingService
{
    Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>();
    bool MappingExistsFor<TSource, TDestination>(string propertyNames);
}