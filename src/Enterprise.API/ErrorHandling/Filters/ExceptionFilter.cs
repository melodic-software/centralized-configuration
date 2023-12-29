using Microsoft.AspNetCore.Mvc.Filters;

namespace Enterprise.API.ErrorHandling.Filters;

// NOTE: Filters have to be registered in the "AddControllers()" method when configuring / adding services.

public class ExceptionFilter : IActionFilter, IOrderedFilter
{
    // These execute in a specific order.
    // This puts it towards the end of the list (unless we have multiple custom filters registered).
    public int Order => int.MaxValue - 10;

    public void OnActionExecuting(ActionExecutingContext context)
    {

    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        //if (context.Exception == null)
        //    return;

        // TODO: Implement a better exception filter that targets specific custom exception types.
        // One example is for API calls to an external provider or service that is unavailable.
        // The status code / error details of the call can be mapped and added here (via custom exception properties).
        // One such status code: StatusCodes.Status424FailedDependency.

        // NOTE: System.Text.Json does not support serialization / deserialization of exception classes.
        // This is the common error: "Serialization and deserialization of 'System.Reflection.MethodBase' instances are not supported. Path: $.TargetSite."

        //context.Result = new ObjectResult(context.Exception)
        //{
        //    StatusCode = 500,
        //};

        // If this isn't set to true, it will attempt to be handled by other components.
        //context.ExceptionHandled = true;
    }
}