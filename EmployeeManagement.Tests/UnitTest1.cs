<<<<<<< Updated upstream
﻿
=======
﻿using Xunit;
using Moq;
>>>>>>> Stashed changes
using EmployeeManagement.Application.DTOs;
using EmployeeManagement.Application.Interfaces;
using EmployeeManagement.Application.Services;
using EmployeeManagement.Domain.Entities;

public class EmployeeServiceTests
{
<<<<<<< Updated upstream
    // ===== UPDATE TESTS =====

    [Fact]
    public async Task UpdateAsync_ValidEmployee_ReturnsTrue()
    {
        var mockRepository = new Mock<IEmployeeRepository>();

        var existingEmployee = new Employee
        {
            EmployeeId = 101,
            EmployeeCode = "EMP101",
            FullName = "Arun Kumar",
            Email = "arun@gmail.com",
            Department = "IT",
            Designation = "Developer",
            JoiningDate = new DateTime(2025, 1, 15),
            ActiveStatus = true
        };

        var request = new UpdateEmployeeRequest
        {
            EmployeeCode = "EMP101",
            FullName = "Arun Raj",
            Email = "arunraj@gmail.com",
            Department = "HR",
            Designation = "Senior Developer",
            JoiningDate = new DateTime(2025, 2, 15),
            ActiveStatus = true
        };

        mockRepository
            .Setup(r => r.GetByIdAsync(101))
            .ReturnsAsync(existingEmployee);

        mockRepository
            .Setup(r => r.UpdateAsync(It.IsAny<Employee>()))
            .ReturnsAsync(true);

        var service = new EmployeeService(mockRepository.Object);

        var result = await service.UpdateAsync(101, request);

        Assert.True(result);

        mockRepository.Verify(
            r => r.GetByIdAsync(101), Times.Once);

        mockRepository.Verify(
            r => r.UpdateAsync(It.Is<Employee>(e =>
                e.EmployeeId == 101 &&
                e.FullName == "Arun Raj" &&
                e.Department == "HR" &&
                e.Email == "arunraj@gmail.com")),
            Times.Once);
=======
    private readonly Mock<IEmployeeRepository> _mockRepository;
    private readonly EmployeeService _employeeService;

    public EmployeeServiceTests()
    {
        _mockRepository = new Mock<IEmployeeRepository>();

        _employeeService =
            new EmployeeService(_mockRepository.Object);
>>>>>>> Stashed changes
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnEmployees()
    {
<<<<<<< Updated upstream
        var mockRepository = new Mock<IEmployeeRepository>();
=======
        // Arrange
        var employees = new List<Employee>
{
    new Employee
    {
        EmployeeId = 1,
        EmployeeCode = "EMP001",
        FullName = "Chandu"
    },
    new Employee
    {
        EmployeeId = 2,
        EmployeeCode = "EMP002",
        FullName = "Rahul"
    }
};

        _mockRepository
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(employees);
>>>>>>> Stashed changes


<<<<<<< Updated upstream
        var service = new EmployeeService(mockRepository.Object);
        var request = new UpdateEmployeeRequest();

        var result = await service.UpdateAsync(999, request);

        Assert.False(result);

        mockRepository.Verify(
            r => r.UpdateAsync(It.IsAny<Employee>()),
            Times.Never);
=======

        // Act
        var result = await _employeeService.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
>>>>>>> Stashed changes
    }

    [Fact]
    public async Task GetAllAsync_ShouldThrowException_WhenDatabaseFails()
    {
<<<<<<< Updated upstream
        var mockRepository = new Mock<IEmployeeRepository>();

        var existingEmployee = new Employee
        {
            EmployeeId = 101,
            EmployeeCode = "EMP101",
            FullName = "Arun Kumar",
            Email = "arun@gmail.com",
            Department = "IT",
            Designation = "Developer",
            JoiningDate = new DateTime(2025, 1, 15),
            ActiveStatus = true
        };

        var request = new UpdateEmployeeRequest
        {
            EmployeeCode = "EMP101",
            FullName = "Arun Raj",
            Email = "arunraj@gmail.com",
            Department = "HR",
            Designation = "Senior Developer",
            JoiningDate = new DateTime(2025, 2, 15),
            ActiveStatus = true
        };

        mockRepository
            .Setup(r => r.GetByIdAsync(101))
            .ReturnsAsync(existingEmployee);

        mockRepository
            .Setup(r => r.UpdateAsync(It.IsAny<Employee>()))
            .ReturnsAsync(false);

        var service = new EmployeeService(mockRepository.Object);

        var result = await service.UpdateAsync(101, request);

        Assert.False(result);

        mockRepository.Verify(
            r => r.UpdateAsync(It.IsAny<Employee>()),
            Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_UpdatesAllProperties()
    {
        var mockRepository = new Mock<IEmployeeRepository>();

        var existingEmployee = new Employee
        {
            EmployeeId = 101,
            EmployeeCode = "EMP101",
            FullName = "Arun Kumar",
            Email = "arun@gmail.com",
            Department = "IT",
            Designation = "Developer",
            JoiningDate = new DateTime(2025, 1, 15),
            ActiveStatus = true
        };

        var request = new UpdateEmployeeRequest
        {
            EmployeeCode = "EMP102",
            FullName = "Arun Raj",
            Email = "arunraj@gmail.com",
            Department = "HR",
            Designation = "Senior Developer",
            JoiningDate = new DateTime(2025, 2, 15),
            ActiveStatus = false
        };

        mockRepository
            .Setup(r => r.GetByIdAsync(101))
            .ReturnsAsync(existingEmployee);

        mockRepository
            .Setup(r => r.UpdateAsync(It.IsAny<Employee>()))
            .ReturnsAsync(true);

        var service = new EmployeeService(mockRepository.Object);

        var result = await service.UpdateAsync(101, request);

        Assert.True(result);

        mockRepository.Verify(
            r => r.UpdateAsync(It.Is<Employee>(e =>
                e.EmployeeId == 101 &&
                e.EmployeeCode == "EMP102" &&
                e.FullName == "Arun Raj" &&
                e.Email == "arunraj@gmail.com" &&
                e.Department == "HR" &&
                e.Designation == "Senior Developer" &&
                e.JoiningDate == new DateTime(2025, 2, 15) &&
                e.ActiveStatus == false)),
            Times.Once);
=======
        // Arrange
        _mockRepository
            .Setup(x => x.GetAllAsync())
            .ThrowsAsync(new Exception("Database connection failed"));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(
            () => _employeeService.GetAllAsync()
        );

        Assert.Equal("Database connection failed", exception.Message);
>>>>>>> Stashed changes
    }

    // ===== POST TESTS =====

    [Fact]
    public async Task CreateAsync_ShouldCreateEmployee_WhenEmployeeCodeIsUnique()
    {
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

        var result = await service.CreateAsync(request);

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

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => service.CreateAsync(request));

        mockRepository.Verify(
            r => r.CreateAsync(It.IsAny<Employee>()),
            Times.Never);
    }

    [Fact]
    public async Task CreateAsync_ShouldMapAllRequestFieldsToEmployee()
    {
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

        await service.CreateAsync(request);

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
