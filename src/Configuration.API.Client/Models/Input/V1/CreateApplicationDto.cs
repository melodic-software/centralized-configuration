using Configuration.API.Client.Models.Input.V1.Abstract;

namespace Configuration.API.Client.Models.Input.V1;
// NOTE: there are input serialization errors with this (probably due to the type derivation)

public class CreateApplicationDto : ApplicationManipulationDto
{
    /// <summary>
    /// The unique identifier of the application.
    /// </summary>
    public Guid? Id { get; set; }

    /// <summary>
    /// The active state of the application.
    /// </summary>
    public bool? IsActive { get; set; }
}