using EmployeeManagement.Domain.Entities;

namespace EmployeeManagement.Application.Interfaces;

public interface IEmployeeRepository
{
    Task<List<Employee>> GetAllAsync();

    Task<Employee?> GetByIdAsync(int id);

    Task<Employee> CreateAsync(Employee employee);

    Task<bool> UpdateAsync(Employee employee);

    Task<bool> DeleteAsync(int id);
}