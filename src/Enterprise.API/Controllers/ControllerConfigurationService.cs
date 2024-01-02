﻿using System.Reflection;
using Enterprise.API.Controllers.Behavior;
using Enterprise.API.Controllers.Formatters;
using Enterprise.API.Controllers.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.API.Controllers;

public static class ControllerConfigurationService
{
    public static void ConfigureControllers(this IServiceCollection services, ControllerConfigurationOptions controllerConfigOptions)
    {
        FormatterConfigurationOptions formatterConfigOptions = controllerConfigOptions.FormatterConfigurationOptions;

        // this registers ONLY the controllers in the service collection and not views or pages
        // because they are not required in most web API projects
        IMvcBuilder builder = services.AddControllers();

        // If controllers exist in other projects, these can be registered here.
        foreach (Type controllerAssemblyType in controllerConfigOptions.ControllerAssemblyTypes)
        {
            Assembly assembly = controllerAssemblyType.Assembly;
            builder.AddApplicationPart(assembly);
        }

        builder.ConfigureFormatters(formatterConfigOptions);

        builder.AddMvcOptions(options =>
        {
            // Content negotiation: https://learn.microsoft.com/en-us/aspnet/core/web-api/advanced/formatting
            options.RespectBrowserAcceptHeader = true;

            // If the client tries to negotiate for the media type the server doesn't support, it will return 406 Not Acceptable.
            options.ReturnHttpNotAcceptable = true;

            // Formats the property names used as error keys.
            options.ModelMetadataDetailsProviders.Add(new SystemTextJsonValidationMetadataProvider());

            List<IInputFormatter> inputFormatters = options.InputFormatters.ToList();

            // NOTE: The default output formatter is the first one in the list.
            List<IOutputFormatter> outputFormatters = options.OutputFormatters.ToList();
            IOutputFormatter? defaultOutputFormatter = outputFormatters.FirstOrDefault();
            Type? defaultOutputFormatterType = defaultOutputFormatter?.GetType();

            // Instead of manually adding these attributes to all controllers / methods, they can be applied globally here via filters.
            // NOTE: Any convention attributes like [ApiConventionType(typeof(DefaultApiConventions))] will be overridden if filters are applied globally.
            options.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status400BadRequest));
            options.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status406NotAcceptable));
            options.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status500InternalServerError));
            options.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status401Unauthorized));

            // In MOST cases, responses will be in either JSON or XML.
            // By default, other response types are visible to swagger (like "text/json" and "text/xml") which are not accurate.
            // These can be overridden at the controller or action level.
            // TODO: Add option to enable/disable XML and configure this dynamically?
            options.Filters.Add(new ProducesAttribute("application/json", "application/xml"));
            
            // If problem details middleware is enabled, these are possible response types.
            // TODO: Make this configurable (if problem details middleware is enabled).
            options.Filters.Add(new ProducesAttribute("application/problem+json", "application/problem+xml"));

            // Register custom filters.
            if (controllerConfigOptions.EnableGlobalAuthorizeFilter)
                options.Filters.Add(new AuthorizeFilter());

            // NOTE: You can pass in authorization policy names to the authorize filter and apply a global authorization policy.
            //options.Filters.Add<ExceptionFilter>();

            // Cache profiles can be configured here.
            // Controllers can refer to the key names here by setting the CacheProfileName property of the ResponseCache attribute.
            //options.CacheProfiles.Add("120SecondsCacheProfile", new() { Duration = 120 });
        });

        builder.ConfigureApiBehavior();
    }
}