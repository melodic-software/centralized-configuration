using System.Reflection;

namespace Enterprise.Reflection.Properties.Abstract;

public interface IPropertyExistenceService
{
    bool TypeHasProperties<T>(string? properties, 
        BindingFlags bindingFlags = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
}