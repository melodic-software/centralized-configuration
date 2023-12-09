using Configuration.API.Client.Models.Input.V1.Validation.Attributes;
using System.ComponentModel.DataAnnotations;
using static Configuration.API.Client.Models.Input.V1.Validation.Constants.ApplicationValidationMessages;

namespace Configuration.API.Client.Models.Input.V1.Abstract;

[NameCannotEqualDescription]
public abstract class ApplicationManipulationModel
{
    /// <summary>
    /// The name of the application.
    /// </summary>
    [Required(ErrorMessage = NameRequired)]
    [MaxLength(100, ErrorMessage = NameMaxLength)]
    public string Name { get; set; } = null!;

    /// <summary>
    /// An optional abbreviated name of the application.
    /// </summary>
    public string? AbbreviatedName { get; set; }

    /// <summary>
    /// An optional brief description of the application.
    /// </summary>
    [MaxLength(500, ErrorMessage = DescriptionMaxLength)]
    public virtual string? Description { get; set; }
}