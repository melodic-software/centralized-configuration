namespace Enterprise.API.Controllers.Options
{
    public class ControllerConfigurationOptions
    {
        /// <summary>
        /// This essentially adds the [Authorize] attribute to all controllers.
        /// </summary>
        public bool EnableGlobalAuthorizeFilter { get; set; } = false;
    }
}
