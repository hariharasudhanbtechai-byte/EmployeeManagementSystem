using EmployeeManagement.Application.DTOs;
using EmployeeManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeService _employeeService;

    public EmployeesController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    // ===== Ganesh - START: POST API endpoint to create an employee =====

    [HttpPost]
    public async Task<ActionResult<EmployeeResponse>> Create(
        CreateEmployeeRequest request)
    {
        try
        {
            var createdEmployee =
                await _employeeService.CreateAsync(request);

            return StatusCode(
                StatusCodes.Status201Created,
                createdEmployee);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }

        // ===== Ganesh - END: POST API endpoint =====
    }
}