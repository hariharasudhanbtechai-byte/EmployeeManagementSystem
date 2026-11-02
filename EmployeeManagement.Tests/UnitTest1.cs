using EmployeeManagement.Application.DTOs;
using EmployeeManagement.Application.Interfaces;
using EmployeeManagement.Application.Services;
using EmployeeManagement.Domain.Entities;
using Moq;

namespace EmployeeManagement.Tests;

public class EmployeeServiceTests
{
    [Fact]
    public async Task CreateAsync_ShouldCreateEmployee_WhenEmployeeCodeIsUnique()
    {
        // Arrange
        var mockRepository = new Mock<IEmployeeRepository>();

        mockRepository
            .Setup(r => r.EmployeeCodeExistsAsync("EMP001"))
            .ReturnsAsync(false);

        mockRepository
            .Setup(r => r.CreateAsync(It.IsAny<Employee>()))
            .ReturnsAsync((Employee employee) =>
            {
                employee.EmployeeId = 1;
                return employee;
            });

        var service = new EmployeeService(mockRepository.Object);

        var request = new CreateEmployeeRequest
        {
            EmployeeCode = "EMP001",
            FullName = "Rahul Sharma",
            Email = "rahul@example.com",
            Department = "IT",
            Designation = "Developer",
            JoiningDate = new DateTime(2026, 10, 1)
        };

        // Act
        var result = await service.CreateAsync(request);

        // Assert
        Assert.Equal(1, result.EmployeeId);
        Assert.Equal("EMP001", result.EmployeeCode);
        Assert.Equal("Rahul Sharma", result.FullName);
        Assert.Equal("rahul@example.com", result.Email);
        Assert.True(result.ActiveStatus);

        mockRepository.Verify(
            r => r.CreateAsync(It.IsAny<Employee>()),
            Times.Once);
    }
    [Fact]
    public async Task CreateAsync_ShouldThrowException_WhenEmployeeCodeAlreadyExists()
    {
        // Arrange
        var mockRepository = new Mock<IEmployeeRepository>();

        mockRepository
            .Setup(r => r.EmployeeCodeExistsAsync("EMP001"))
            .ReturnsAsync(true);

        var service = new EmployeeService(mockRepository.Object);

        var request = new CreateEmployeeRequest
        {
            EmployeeCode = "EMP001",
            FullName = "Rahul Sharma",
            Email = "rahul@example.com",
            Department = "IT",
            Designation = "Developer",
            JoiningDate = new DateTime(2026, 10, 1)
        };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(
            () => service.CreateAsync(request));

        // Verify that employee creation was never attempted
        mockRepository.Verify(
            r => r.CreateAsync(It.IsAny<Employee>()),
            Times.Never);
    }
    [Fact]
    public async Task CreateAsync_ShouldMapAllRequestFieldsToEmployee()
    {
        // Arrange
        var mockRepository = new Mock<IEmployeeRepository>();
        Employee? capturedEmployee = null;

        mockRepository
            .Setup(r => r.EmployeeCodeExistsAsync("EMP003"))
            .ReturnsAsync(false);

        mockRepository
            .Setup(r => r.CreateAsync(It.IsAny<Employee>()))
            .Callback<Employee>(employee => capturedEmployee = employee)
            .ReturnsAsync((Employee employee) => employee);

        var service = new EmployeeService(mockRepository.Object);

        var joiningDate = new DateTime(2026, 10, 15);

        var request = new CreateEmployeeRequest
        {
            EmployeeCode = "EMP003",
            FullName = "Priya Kumar",
            Email = "priya@example.com",
            Department = "HR",
            Designation = "HR Executive",
            JoiningDate = joiningDate
        };

        // Act
        await service.CreateAsync(request);

        // Assert
        Assert.NotNull(capturedEmployee);
        Assert.Equal("EMP003", capturedEmployee.EmployeeCode);
        Assert.Equal("Priya Kumar", capturedEmployee.FullName);
        Assert.Equal("priya@example.com", capturedEmployee.Email);
        Assert.Equal("HR", capturedEmployee.Department);
        Assert.Equal("HR Executive", capturedEmployee.Designation);
        Assert.Equal(joiningDate, capturedEmployee.JoiningDate);
        Assert.True(capturedEmployee.ActiveStatus);
    }
}

