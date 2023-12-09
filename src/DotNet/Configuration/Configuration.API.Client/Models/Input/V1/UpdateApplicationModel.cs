using Configuration.API.Client.Models.Input.V1.Abstract;
using System.ComponentModel.DataAnnotations;
using static Configuration.API.Client.Models.Input.V1.Validation.Constants.ApplicationValidationMessages;

namespace Configuration.API.Client.Models.Input.V1;

public class UpdateApplicationModel : ApplicationManipulationModel
{
    [Required(ErrorMessage = IsActiveRequired)]
    public bool IsActive { get; set; }

    // this is an example of a field that is required for updates, but not required for resource creation

    [Required(ErrorMessage = DescriptionRequired)]
    public override string? Description
    {
        get => base.Description;
        set => base.Description = value;
    }
}