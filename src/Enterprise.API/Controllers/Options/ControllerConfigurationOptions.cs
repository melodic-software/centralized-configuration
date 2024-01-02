namespace Enterprise.API.Controllers.Options;

public class ControllerConfigurationOptions
{
    /// <summary>
    /// This essentially adds the [Authorize] attribute to all controllers.
    /// </summary>
    public bool EnableGlobalAuthorizeFilter { get; set; } = true;

    /// <summary>
    /// If you have controllers in another project, you can add one or more type references here.
    /// Typically, this is a custom static class called "AssemblyReference" with no implementation.
    /// The type reference is used to get the assembly reference, which is added as an application part.
    /// </summary>
    public List<Type> ControllerAssemblyTypes { get; set; } = new();

    /// <summary>
    /// Options for configuring input and output formatters.
    /// </summary>
    public FormatterConfigurationOptions FormatterConfigurationOptions { get; set; } = new();
}