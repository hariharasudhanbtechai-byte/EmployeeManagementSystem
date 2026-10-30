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
    //SAI - GetByID
   public async Task<EmployeeResponse?> GetByIdAsync(int id)
    {
        var employee = await _employeeRepository.GetByIdAsync(id);

        if (employee is null)
            return null;

        return new EmployeeResponse
        {
            EmployeeId = employee.EmployeeId,
            EmployeeCode = employee.EmployeeCode,
            FullName = employee.FullName,
            Email = employee.Email,
            Department = employee.Department,
            Designation = employee.Designation,
            JoiningDate = employee.JoiningDate,
            ActiveStatus = employee.ActiveStatus
        };
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


    //Start By vishnu -- Get
    public async Task<List<EmployeeResponse>> GetAllAsync()
    {
        try
        {
            var employees = await _employeeRepository.GetAllAsync();

            if (employees == null)
            {
                throw new KeyNotFoundException("No employee Found.");
            }

            var result = employees.Select(s => new EmployeeResponse
            {
                EmployeeId = s.EmployeeId,
                EmployeeCode = s.EmployeeCode,
                FullName = s.FullName,
                Email = s.Email,
                Department = s.Department,
                Designation = s.Designation,
                JoiningDate = s.JoiningDate,
                ActiveStatus = s.ActiveStatus
            }).ToList();

            return result;
        }
        catch (KeyNotFoundException)
        {
            throw;
        }
    }

}
