using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace CompanyEmployees.Presentation.Controllers
{
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
    }
}
