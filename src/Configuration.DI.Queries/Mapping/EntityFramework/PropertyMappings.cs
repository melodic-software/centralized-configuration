using Configuration.API.Client.Models.Output.V1;
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
        { nameof(ApplicationModel.Id), new SortablePropertyMappingValue(new List<string> {nameof(ApplicationEntity.ApplicationId)}) },
        { nameof(ApplicationModel.UniqueName), new SortablePropertyMappingValue(new List<string> {nameof(ApplicationEntity.UniqueName) }) },
        { nameof(ApplicationModel.Name), new SortablePropertyMappingValue(new List<string> {nameof(ApplicationEntity.Name) }) },
        { nameof(ApplicationModel.AbbreviatedName), new SortablePropertyMappingValue(new List<string> {nameof(ApplicationEntity.AbbreviatedName) }) },
        { nameof(ApplicationModel.Description), new SortablePropertyMappingValue(new List<string> {nameof(ApplicationEntity.Description) }) },
        { nameof(ApplicationModel.IsActive), new SortablePropertyMappingValue(new List<string> {nameof(ApplicationEntity.IsActive) }) }
    };
}