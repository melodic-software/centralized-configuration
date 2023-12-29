using Configuration.API.Client.DTOs.Output.V1;
using Configuration.EntityFramework.Entities;
using Enterprise.Mapping.Properties.Model;
using Enterprise.Sorting.Model;

namespace Configuration.DI.Queries.Mapping.EntityFramework;
// NOTE: this same technique can be applied to filtering
// don't forget that query models, or API model contracts could contain different field representations (ex: concatenated, calculated, etc.)
// the property mapping dictionary provides the translation

internal class PropertyMappings
{
    internal static readonly Dictionary<string, PropertyMappingValue> ApplicationMappings = new(StringComparer.OrdinalIgnoreCase)
    {
        // these are translations of the API model contract / resource representation to the underlying data store properties
        // these can vary by name, or can be calculated or concatenated model properties
        { nameof(ApplicationDto.Id), new SortablePropertyMappingValue(new List<string> {nameof(ApplicationEntity.ApplicationId)}) },
        { nameof(ApplicationDto.UniqueName), new SortablePropertyMappingValue(new List<string> {nameof(ApplicationEntity.UniqueName) }) },
        { nameof(ApplicationDto.Name), new SortablePropertyMappingValue(new List<string> {nameof(ApplicationEntity.Name) }) },
        { nameof(ApplicationDto.AbbreviatedName), new SortablePropertyMappingValue(new List<string> {nameof(ApplicationEntity.AbbreviatedName) }) },
        { nameof(ApplicationDto.Description), new SortablePropertyMappingValue(new List<string> {nameof(ApplicationEntity.Description) }) },
        { nameof(ApplicationDto.IsActive), new SortablePropertyMappingValue(new List<string> {nameof(ApplicationEntity.IsActive) }) }
    };
}