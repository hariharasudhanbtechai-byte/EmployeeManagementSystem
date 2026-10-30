using EmployeeManagement.Application.Interfaces;
using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Infrastructure.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly EmployeeDbContext _context;

    public EmployeeRepository(EmployeeDbContext context)
    {
        _context = context;
    }

    // ===== Ganesh - START: Check employee code duplication =====
    public async Task<bool> EmployeeCodeExistsAsync(
       string employeeCode)
    {
        return await _context.Employees
            .AnyAsync(e => e.EmployeeCode == employeeCode);
    }

    
    public async Task<Employee> CreateAsync(Employee employee)
    {
        await _context.Employees.AddAsync(employee);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
            when (ex.InnerException is SqlException sqlException
                  && (sqlException.Number == 2601
                      || sqlException.Number == 2627))
        {
            throw new InvalidOperationException(
                $"Employee code '{employee.EmployeeCode}' already exists.",
                ex);
        }

        return employee;
    }
    // ===== Ganesh - START: Check employee code duplication =====
    public async Task<bool> UpdateAsync(Employee employee)
    {
        var existingEmployee = await _context.Employees
            .FirstOrDefaultAsync(e => e.EmployeeId == employee.EmployeeId);

        if (existingEmployee == null)
        {
            return false;
        }

        existingEmployee.EmployeeCode = employee.EmployeeCode;
        existingEmployee.FullName = employee.FullName;
        existingEmployee.Email = employee.Email;
        existingEmployee.Department = employee.Department;
        existingEmployee.Designation = employee.Designation;
        existingEmployee.JoiningDate = employee.JoiningDate;
        existingEmployee.ActiveStatus = employee.ActiveStatus;

        await _context.SaveChangesAsync();

        return true;
    }

    //Start By Vishnu -- Get All Employee
    public async Task<List<Employee>> GetAllAsync()
    {
        return await _context.Employees.ToListAsync();
    }
    //End of Get All Employee
}