using EmployeeManagement.Application.DTOs;
using EmployeeManagement.Application.Interfaces;
using EmployeeManagement.Domain.Entities;

namespace EmployeeManagement.Application.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeeService(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    //Ganesh Create
    public async Task<EmployeeResponse> CreateAsync(
        CreateEmployeeRequest request)
    {
        var employeeCodeExists =
            await _employeeRepository.EmployeeCodeExistsAsync(
                request.EmployeeCode);

        if (employeeCodeExists)
        {
            throw new InvalidOperationException(
                $"Employee code '{request.EmployeeCode}' already exists.");
        }

        var employee = new Employee
        {
            EmployeeCode = request.EmployeeCode,
            FullName = request.FullName,
            Email = request.Email,
            Department = request.Department,
            Designation = request.Designation,
            JoiningDate = request.JoiningDate!.Value,
            ActiveStatus = true
        };

        var createdEmployee =
            await _employeeRepository.CreateAsync(employee);

        return new EmployeeResponse
        {
            EmployeeId = createdEmployee.EmployeeId,
            EmployeeCode = createdEmployee.EmployeeCode,
            FullName = createdEmployee.FullName,
            Email = createdEmployee.Email,
            Department = createdEmployee.Department,
            Designation = createdEmployee.Designation,
            JoiningDate = createdEmployee.JoiningDate,
            ActiveStatus = createdEmployee.ActiveStatus
        };
    }
    public async Task<bool> UpdateAsync(
        int id,
        UpdateEmployeeRequest request)
    {
        var employee = await _employeeRepository.GetByIdAsync(id);

        if (employee == null)
        {
            return false;
        }

        employee.EmployeeCode = request.EmployeeCode;
        employee.FullName = request.FullName;
        employee.Email = request.Email;
        employee.Department = request.Department;
        employee.Designation = request.Designation;
        employee.JoiningDate = request.JoiningDate;
        employee.ActiveStatus = request.ActiveStatus;

        return await _employeeRepository.UpdateAsync(employee);
    }
}