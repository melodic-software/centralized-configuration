using AutoMapper;
using Configuration.API.Client.Models.Input.V1;
using Configuration.API.Client.Models.Output.V1;
using Configuration.ApplicationServices.Queries.Applications;
using Configuration.ApplicationServices.Queries.Applications.Results;
using Configuration.Core.Queries.Model;
using Enterprise.API.Client.Hypermedia;
using Enterprise.API.Client.Hypermedia.Constants;
using Enterprise.API.Client.Pagination;
using Enterprise.API.ContentNegotiation.Extensions;
using Enterprise.API.Controllers.Extensions;
using Enterprise.API.Pagination;
using Enterprise.API.Results;
using Enterprise.Core.Queries.Paging;
using Enterprise.DataShaping.Extensions;
using Enterprise.Events.Model.Domain.Concrete;
using Enterprise.Reflection.Properties.Abstract;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using System.Dynamic;
using Enterprise.ApplicationServices.Queries.Handlers.Generic;

namespace Configuration.API.Controllers.Applications.V1.Extensions;

public static class QueryExtensions
{
    public static async Task<DataShapedQueryResult> GetApplications(this ControllerBase controller, GetApplicationsModel model,
        IPropertyExistenceService propertyExistenceService, IHandleQuery<GetApplications, GetApplicationsResult> queryHandler, IMapper mapper,
        ILogger logger, ProblemDetailsFactory problemDetailsFactory)
    {
        // here's some additional options for data shaping and filters:
        // https://app.pluralsight.com/course-player?clipId=1b109e50-d99a-4d6f-998f-5a4505539c21

        logger.LogInformation("Getting applications");

        // TODO: return the properties not found? this would make it easier on the client to fix the error
        // right now it returns all properties sent in the request, so you don't know which one is wrong
        if (!propertyExistenceService.TypeHasProperties<ApplicationModel>(model.Properties))
        {
            IActionResult actionResult = controller.BadDataShapingRequest(problemDetailsFactory, model.Properties);
            return DataShapedQueryResult.Failure(actionResult);
        }

        // map from the API model contract to the application services (query) model
        GetApplications query = mapper.Map<GetApplications>(model);

        List<ValidationFailure> validationFailures = new List<ValidationFailure>();

        // execute the query
        queryHandler.RegisterEventCallback(new Action<ValidationFailure>(validationFailures.Add));
        GetApplicationsResult queryResult = await queryHandler.HandleAsync(query);

        // this can happen if the client specified invalid sort parameters
        if (validationFailures.Any())
        {
            IActionResult actionResult = controller.BadRequest();
            return DataShapedQueryResult.Failure(actionResult);
        }

        // query results
        IEnumerable<Application> applications = queryResult.Applications;
        PaginationMetadata paginationMetadata = queryResult.PaginationMetadata;

        logger.LogInformation($"Returning {applications.Count()} applications.");

        // create pagination metadata and assign to the response (via a custom header)
        PagingMetadataModel pagingMetadataModel = mapper.Map<PagingMetadataModel>(paginationMetadata);
        PaginationResponseHeaderService.AddToResponseHeader(pagingMetadataModel, controller);

        // map from the core query objects to API model contracts
        IEnumerable<ApplicationModel>? mapResult = mapper.Map<IEnumerable<ApplicationModel>>(applications);

        // only return the fields specified by the client (if applicable)
        // this makes the resulting API model contract dynamic (data shaping)
        IEnumerable<ExpandoObject> dataShapedResult = mapResult.ShapeData(model.Properties);

        DataShapedQueryResult result = DataShapedQueryResult.Success(dataShapedResult, paginationMetadata);

        return result;
    }

    public static ExpandoObject GetResultModel(this ControllerBase controller, Application application,
        string? properties, MediaTypeHeaderValue mediaType, IMapper mapper)
    {
        bool includeLinks = mediaType.EndsWithHATEOAS();

        IEnumerable<HypermediaLinkModel> links = new List<HypermediaLinkModel>();

        if (includeLinks)
        {
            // create the hypermedia links
            links = controller.CreateLinksForApplication(application.Id, properties);
        }

        StringSegment primaryMediaType = mediaType.GetPrimaryMediaType(includeLinks);

        // if there are different representations that can be returned based on vendor media types
        // handle those here by checking the "primary media type"

        // this is the default API model 
        // this is the concrete model type that we data shape (becomes dynamic) and return
        ApplicationModel? mapResult = mapper.Map<ApplicationModel>(application);

        // apply data shaping (if applicable), which results in a dynamic model contract
        ExpandoObject resultModel = mapResult.ShapeData(properties);

        if (includeLinks)
        {
            // add the hypermedia links to the result model
            IDictionary<string, object?> modelDictionary = resultModel;
            modelDictionary.Add(HypermediaConstants.RootPropertyName, links);
        }

        return resultModel;
    }
}