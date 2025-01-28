using System;
using Xunit;

public class BackgroundTimeServiceTests
{
    [Fact]
    public void CheckTime_ShouldPerformMaintenance_WhenTimeIsAfterStartTime()
    {
        // Arrange: Set fake time to 22:00 (10 PM)
        var fakeTime = new DateTime(2025, 1, 27, 22, 0, 0);
        var service = new BackgroundTimeService(() => fakeTime);

        bool maintenanceCalled = false;
        service.PerformMaintenanceAction = () => maintenanceCalled = true;

        // Act
        service.CheckTime();

        // Assert
        Assert.True(maintenanceCalled, "Maintenance should be performed when time is within range.");
    }

    [Fact]
    public void CheckTime_ShouldNotPerformMaintenance_WhenAlreadyLoggedToday()
    {
        // Arrange: Set fake time to 22:00 (10 PM)
        var fakeTime = new DateTime(2025, 1, 27, 22, 0, 0);
        var service = new BackgroundTimeService(() => fakeTime);

        int maintenanceCallCount = 0;
        service.PerformMaintenanceAction = () => maintenanceCallCount++;

        // Act: Call twice, it should only trigger once
        service.CheckTime();
        service.CheckTime();

        // Assert
        Assert.Equal(1, maintenanceCallCount);
    }

    [Fact]
    public void CheckTime_ShouldResetAt5AM()
    {
        // Arrange: Set fake time to 5:00 AM to trigger reset
        var fakeTime = new DateTime(2025, 1, 27, 5, 0, 0);
        var service = new BackgroundTimeService(() => fakeTime);

        bool resetTriggered = false;
        service.OnDailyReset += () => resetTriggered = true;

        // Act
        service.CheckTime();

        // Assert
        Assert.True(resetTriggered, "Reset should be triggered at 5 AM.");
    }

    [Fact]
    public void CheckTime_ShouldNotPerformMaintenance_OutsideValidTimeRange()
    {
        // Arrange: Set fake time to 10:00 AM (outside valid range)
        var fakeTime = new DateTime(2025, 1, 27, 10, 0, 0);
        var service = new BackgroundTimeService(() => fakeTime);

        bool maintenanceCalled = false;
        service.PerformMaintenanceAction = () => maintenanceCalled = true;

        // Act
        service.CheckTime();

        // Assert
        Assert.False(maintenanceCalled, "Maintenance should not be performed outside valid range.");
    }

    [Fact]
    public void CheckTime_ShouldPerformMaintenance_BeforeEndTimeAtMidnight()
    {
        // Arrange: Set fake time to 00:30 AM (midnight period)
        var fakeTime = new DateTime(2025, 1, 27, 0, 30, 0);
        var service = new BackgroundTimeService(() => fakeTime);

        bool maintenanceCalled = false;
        service.PerformMaintenanceAction = () => maintenanceCalled = true;

        // Act
        service.CheckTime();

        // Assert
        Assert.True(maintenanceCalled, "Maintenance should be performed before 1 AM.");
    }

    [Fact]
    public void CheckTime_ShouldResetLoggingFlag_At6AM()
    {
        // Arrange: Set fake time to 6:00 AM
        var fakeTime = new DateTime(2025, 1, 27, 6, 0, 0);
        var service = new BackgroundTimeService(() => fakeTime);

        bool resetTriggered = false;
        service.OnDailyReset += () => resetTriggered = true;

        // Act
        service.CheckTime();

        // Assert
        Assert.True(resetTriggered, "Logging flag should reset at 6 AM.");
    }
}
