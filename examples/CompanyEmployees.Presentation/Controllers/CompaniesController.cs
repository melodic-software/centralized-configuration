
using Enterprise.API.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace CompanyEmployees.Presentation.Controllers;

// The [ApiController] attribute enables the following opinionated, API-specific behaviors:
// Attribute routing requirement
// Automatic HTTP 400 responses
// Binding source parameter inference
// Multipart/form-data request inference
// Problem details for error status codes

[Route("api/companies")]
[ApiController]
public class CompaniesController(IServiceManager service) : ControllerBase
{
    [HttpGet]
    public IActionResult GetCompanies()
    {
        IEnumerable<CompanyDto> companies = service.CompanyService.GetAllCompanies(trackChanges: false);
        return Ok(companies);
    }

    [HttpGet("{id:guid}", Name = "CompanyById")]
    public IActionResult GetCompany(Guid id)
    {
        CompanyDto company = service.CompanyService.GetCompany(id, trackChanges: false);

        return Ok(company);
    }

    [HttpGet("collection/({ids})", Name = "CompanyCollection")]
    public IActionResult GetCompanyCollection([ModelBinder(BinderType = typeof(GenericEnumerableModelBinder))] IEnumerable<Guid> ids)
    {
        IEnumerable<CompanyDto> companies = service.CompanyService.GetByIds(ids, trackChanges: false);

        return Ok(companies);
    }

    [HttpPost]
    public IActionResult CreateCompany([FromBody] CompanyForCreationDto company)
    {
        if (company is null)
            return BadRequest("CompanyForCreationDto object is null");

        CompanyDto createdCompany = service.CompanyService.CreateCompany(company);

        // Populates the location attribute within the response header with the address to retrieve the company.
        return CreatedAtRoute("CompanyById", new { id = createdCompany.Id }, createdCompany);
    }

    [HttpPost("collection")]
    public IActionResult CreateCompanyCollection([FromBody] IEnumerable<CompanyForCreationDto> companyCollection)
    {
        (IEnumerable<CompanyDto> companies, string ids) result = service.CompanyService
            .CreateCompanyCollection(companyCollection);

        var routeValues = new { result.ids };

        return CreatedAtRoute("CompanyCollection", routeValues, result.companies);
    }
}