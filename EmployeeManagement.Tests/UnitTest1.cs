using EmployeeManagement.Application.DTOs;
using EmployeeManagement.Application.Interfaces;
using EmployeeManagement.Application.Services;
using EmployeeManagement.Domain.Entities;
using Moq;

namespace EmployeeManagement.Tests;

public class EmployeeServiceTests
{
    [Fact]
    public async Task UpdateAsync_ValidEmployee_ReturnsTrue()
    {
        // Arrange

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


        // Act

        var result = await service.UpdateAsync(101, request);


        // Assert

        Assert.True(result);

        mockRepository.Verify(
            r => r.GetByIdAsync(101),
            Times.Once);

        mockRepository.Verify(
            r => r.UpdateAsync(It.Is<Employee>(e =>
                e.EmployeeId == 101 &&
                e.FullName == "Arun Raj" &&
                e.Department == "HR" &&
                e.Email == "arunraj@gmail.com")),
            Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_EmployeeNotFound_ReturnsFalse()
    {
        // Arrange

        var mockRepository = new Mock<IEmployeeRepository>();

        mockRepository
            .Setup(r => r.GetByIdAsync(999))
            .ReturnsAsync((Employee?)null);

        var service = new EmployeeService(mockRepository.Object);

        var request = new UpdateEmployeeRequest();

        // Act

        var result = await service.UpdateAsync(999, request);

        // Assert

        Assert.False(result);

        mockRepository.Verify(
            r => r.UpdateAsync(It.IsAny<Employee>()),
            Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_RepositoryReturnsFalse_ReturnsFalse()
    {
        // Arrange

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

        // Act

        var result = await service.UpdateAsync(101, request);

        // Assert

        Assert.False(result);

        mockRepository.Verify(
            r => r.UpdateAsync(It.IsAny<Employee>()),
            Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_UpdatesAllProperties()
    {
        // Arrange

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

        // Act

        var result = await service.UpdateAsync(101, request);

        // Assert

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
    }
}