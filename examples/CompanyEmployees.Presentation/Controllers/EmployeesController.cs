using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace CompanyEmployees.Presentation.Controllers;

[Route("api/companies/{companyId}/employees")]
[ApiController]
public class EmployeesController(IServiceManager service) : ControllerBase
{
    [HttpGet]
    public IActionResult GetEmployeesForCompany(Guid companyId)
    {
        IEnumerable<EmployeeDto> employees = service.EmployeeService.GetEmployees(companyId, trackChanges: false);

        return Ok(employees);
    }

    [HttpGet("{id:guid}", Name = "GetEmployeeForCompany")]
    public IActionResult GetEmployeeForCompany(Guid companyId, Guid id)
    {
        EmployeeDto employee = service.EmployeeService.GetEmployee(companyId, id, trackChanges: false);

        return Ok(employee);
    }

    [HttpPost]
    public IActionResult CreateEmployeeForCompany(Guid companyId, [FromBody] EmployeeForCreationDto? employee)
    {
        if (employee is null)
            return BadRequest("EmployeeForCreationDto object is null");

        EmployeeDto employeeToReturn = service.EmployeeService
            .CreateEmployeeForCompany(companyId, employee, trackChanges: false);

        var routeValues = new { companyId, id = employeeToReturn.Id };

        return CreatedAtRoute("GetEmployeeForCompany", routeValues, employeeToReturn);
    }
}