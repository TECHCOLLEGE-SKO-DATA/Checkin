using CheckinLibrary.Database;
using CheckinLibrary.Models;
using CheckInSystem.Platform;
using CheckInSystem.Tests.Platform;
using CheckInSystem.ViewModels.Windows;
using Metsys.Bson;
using System;
using Xunit;

namespace BackgroundTimeServiceTests;
public class BackgroundTimeServiceTests
{
    TestPlatform testPlatform;
    /*
    [Fact]
    public void CheckTime_ShouldPerformMaintenance_WhenTimeIsAfterStartTime()
    {
        // Arrange
        var fakeTime = new DateTime(2025, 1, 27, 3, 0, 0);
        var service = new BackgroundTimeService(() => fakeTime);
        EmployeeOverviewViewModel employeeOverviewViewModel = new(testPlatform);

        // Provide fake employees
        service.GetEmployees = () => new List<Employee>
        {
            new Employee(1, "Test", false)
        };

        bool maintenanceCalled = false;
        service.PerformMaintenanceAction = (employees) => maintenanceCalled = true;

        // Act
        service.CheckTime(employeeOverviewViewModel);

        // Assert
        Assert.True(maintenanceCalled, "Maintenance should be performed when time is within range.");
    }


    [Fact]
    public void CheckTime_ShouldNotPerformMaintenance_WhenAlreadyLoggedToday()
    {
        // Arrange: Fake time
        var fakeTime = new DateTime(2025, 1, 27, 1, 0, 0);
        var service = new BackgroundTimeService(() => fakeTime);
        EmployeeOverviewViewModel employeeOverviewViewModel = new(testPlatform);

        // Provide fake employees
        service.GetEmployees = () => new List<Employee>
        {
            new Employee(1, "Test", false)
        };

        int maintenanceCallCount = 0;
        service.PerformMaintenanceAction = (employees) => maintenanceCallCount++;

        // Act: Call twice
        service.CheckTime(employeeOverviewViewModel);
        service.CheckTime(employeeOverviewViewModel);

        // Assert
        Assert.Equal(1, maintenanceCallCount);
    }


    [Fact]
    public void CheckTime_ShouldResetAt5AM()
    {
        // Arrange: Set fake time to 5:00 AM to trigger reset
        var fakeTime = new DateTime(2025, 1, 27, 5, 0, 0);
        var service = new BackgroundTimeService(() => fakeTime);

        // Provide fake employees
        service.GetEmployees = () => new List<Employee>
        {
            new Employee(1, "Test", false)
        };

        bool resetTriggered = false;
        service.OnDailyReset += () => resetTriggered = true;
        EmployeeOverviewViewModel employeeOverviewViewModel = new(testPlatform);

        // Act
        service.CheckTime(employeeOverviewViewModel);

        // Assert
        Assert.True(resetTriggered, "Reset should be triggered at 5 AM.");
    }

    [Fact]
    public void CheckTime_ShouldNotPerformMaintenance_OutsideValidTimeRange()
    {
        // Arrange: Set fake time to 10:00 AM (outside valid range)
        var fakeTime = new DateTime(2025, 1, 27, 10, 0, 0);
        var service = new BackgroundTimeService(() => fakeTime);
        EmployeeOverviewViewModel employeeOverviewViewModel = new(testPlatform);

        // Provide fake employees
        service.GetEmployees = () => new List<Employee>
        {
            new Employee(1, "Test", false)
        };

        bool maintenanceCalled = false;
        service.PerformMaintenanceAction = (employees) => maintenanceCalled = true;

        // Act
        service.CheckTime(employeeOverviewViewModel);

        // Assert
        Assert.False(maintenanceCalled, "Maintenance should not be performed outside valid range.");
    }

    [Fact]
    public void CheckTime_ShouldPerformMaintenance_BeforeEndTimeAtMidnight()
    {
        // Arrange: Set fake time to 01:30 AM
        var fakeTime = new DateTime(2025, 1, 27, 1, 30, 0);
        var service = new BackgroundTimeService(() => fakeTime);
        EmployeeOverviewViewModel employeeOverviewViewModel = new(testPlatform);

        // Provide fake employees
        service.GetEmployees = () => new List<Employee>
        {
            new Employee(1, "Test", false)
        };

        bool maintenanceCalled = false;
        service.PerformMaintenanceAction = (employees) => maintenanceCalled = true;

        // Act
        service.CheckTime(employeeOverviewViewModel);

        // Assert
        Assert.True(maintenanceCalled, "Maintenance should be performed before 1 AM.");
    }

    [Fact]
    public void CheckTime_ShouldResetLoggingFlag_At6AM()
    {
        // Arrange: Set fake time to 6:00 AM
        var fakeTime = new DateTime(2025, 1, 27, 6, 0, 0);
        var service = new BackgroundTimeService(() => fakeTime);
        EmployeeOverviewViewModel employeeOverviewViewModel = new(testPlatform);

        // Provide fake employees
        service.GetEmployees = () => new List<Employee>
        {
            new Employee(1, "Test", false)
        };

        bool resetTriggered = false;
        service.OnDailyReset += () => resetTriggered = true;

        // Act
        service.CheckTime(employeeOverviewViewModel);

        // Assert
        Assert.True(resetTriggered, "Logging flag should reset at 6 AM.");
    }
    */
}
