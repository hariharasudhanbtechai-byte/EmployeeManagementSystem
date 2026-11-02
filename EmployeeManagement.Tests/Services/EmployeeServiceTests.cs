
using EmployeeManagement.Application.Interfaces;
using EmployeeManagement.Application.Services;
using EmployeeManagement.Domain.Entities;
using Moq;
using System.Timers;
using Xunit;

namespace EmployeeManagement.Tests.Services;

public class EmployeeServiceTests
{
    private readonly Mock<IEmployeeRepository> _mockRepository;
    private readonly EmployeeService _employeeService;

    public EmployeeServiceTests()
    {
        _mockRepository = new Mock<IEmployeeRepository>();

        _employeeService = new EmployeeService(
            _mockRepository.Object
        );
    }

    // TEST CASE 1: Employee exists

    [Fact]
    public async Task GetByIdAsync_ReturnsEmployee_WhenEmployeeExists()
    {
        // Arrange
        var employee = new Employee
        {
            EmployeeId = 1,
            EmployeeCode = "EMP001",
            FullName = "Sai Kumar",
            Email = "sai@example.com",
            Department = "IT",
            Designation = "Software Developer Intern",
            JoiningDate = new DateTime(2026, 10, 29),
            ActiveStatus = true
        };

        _mockRepository
            .Setup(repo => repo.GetByIdAsync(1))
            .ReturnsAsync(employee);

        // Act
        var result = await _employeeService.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.EmployeeId);
        Assert.Equal("EMP001", result.EmployeeCode);
        Assert.Equal("Sai Kumar", result.FullName);
        Assert.Equal("sai@example.com", result.Email);
        Assert.Equal("IT", result.Department);
        Assert.Equal("Software Developer Intern", result.Designation);
        Assert.True(result.ActiveStatus);

        _mockRepository.Verify(
            repo => repo.GetByIdAsync(1),
            Times.Once
        );
    }

    // TEST CASE 2: Employee does not exist

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenEmployeeDoesNotExist()
    {
        // Arrange
        _mockRepository
            .Setup(repo => repo.GetByIdAsync(100))
            .ReturnsAsync((Employee?)null);

        // Act
        var result = await _employeeService.GetByIdAsync(100);

        // Assert
        Assert.Null(result);

        _mockRepository.Verify(
            repo => repo.GetByIdAsync(100),
            Times.Once
        );
    }

    // TEST CASE 3: Verify repository is called once

    [Fact]
    public async Task GetByIdAsync_CallsRepositoryOnce()
    {
        // Arrange
        _mockRepository
            .Setup(repo => repo.GetByIdAsync(2))
            .ReturnsAsync((Employee?)null);

        // Act
        await _employeeService.GetByIdAsync(2);

        // Assert
        _mockRepository.Verify(
            repo => repo.GetByIdAsync(2),
            Times.Once
        );
    }
}
