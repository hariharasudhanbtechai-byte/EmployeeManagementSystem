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

    // 1. GET - Get All Employees
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<EmployeeResponse>>> GetAll()
    {
        var employees = await _employeeService.GetAllAsync();

        return Ok(employees);
    }

    // 2. GET BY ID - Get Employee by ID
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<EmployeeResponse>> GetById(int id)
    {
        if (id <= 0)
            return BadRequest(new { message = "Invalid Employee ID." });

        var employee = await _employeeService.GetByIdAsync(id);

        if (employee == null)
            return NotFound(new { message = "Employee not found." });

        return Ok(employee);
    }

    // 3. POST - Create Employee
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
    }

    // 4. PUT - Update Employee
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update(
        int id,
        UpdateEmployeeRequest request)
    {
        if (id <= 0)
            return BadRequest(new { message = "Invalid Employee ID." });

        var result = await _employeeService.UpdateAsync(id, request);

        if (!result)
            return NotFound(new { message = "Employee not found." });

        return Ok(new { message = "Employee updated successfully." });
    }

    // 5. DELETE - Delete Employee by ID
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(int id)
    {
        if (id <= 0)
            return BadRequest(new { message = "Invalid Employee ID." });

        var result = await _employeeService.DeleteEmployeeAsync(id);
          
        if (!result)
            return NotFound(new { message = "Employee not found." });

        return Ok(new { message = "Employee deleted successfully." });
    }
}
