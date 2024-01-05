using Configuration.ApplicationServices.Applications.CreateApplication;
using FluentValidation;

namespace Configuration.ApplicationServices.FluentValidation;

public class CreateApplicationValidator : AbstractValidator<CreateApplication>
{
    public CreateApplicationValidator()
    {
        RuleFor(c => c.Id)
            .NotNull()
            .NotEmpty()
            .NotEqual(Guid.Empty);
            
        RuleFor(c => c.Name)
            .NotNull()
            .NotEmpty();
    }
}