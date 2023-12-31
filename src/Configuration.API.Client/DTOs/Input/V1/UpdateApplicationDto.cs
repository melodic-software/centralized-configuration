﻿using System.ComponentModel.DataAnnotations;
using Configuration.API.Client.DTOs.Input.V1.Abstract;
using static Configuration.API.Client.DTOs.Input.V1.Validation.Constants.ApplicationValidationMessages;

namespace Configuration.API.Client.DTOs.Input.V1;

public class UpdateApplicationDto : ApplicationManipulationDto
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