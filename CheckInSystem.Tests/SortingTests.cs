using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xunit;
using CheckinLibrary.Models;

namespace CheckInSystem.Tests
{
    public class Sorting_tests
    {
        private static List<Employee> GetMockEmployees()
        {
            return new List<Employee>
            {
                new Employee(1, "Alice", true),
                new Employee(2, "Bob", false),
                new Employee(3, "Charlie", true),
                new Employee(4, "David", false),
            };
        }

        public static List<Group> GetMockGroups(List<Employee> employees)
        {
            var group1 = new Group(1, "Group 1");
            group1.InitializeMembers(employees.Take(2));

            var group2 = new Group(2, "Group 2");
            group2.InitializeMembers(employees.Skip(2));

            return new List<Group> { group1, group2 };
        }

        [Fact]
        public void GroupsAndMembers_ShouldBeInitializedCorrectly()
        {
            // Arrange
            var employees = GetMockEmployees();
            var groups = GetMockGroups(employees);

            // Act
            var viewModelGroups = new ObservableCollection<Group>(groups);

            // Assert
            Assert.Equal(2, viewModelGroups.Count);
            Assert.Equal(2, viewModelGroups[0].Members.Count);
            Assert.Equal(2, viewModelGroups[1].Members.Count);
            Assert.Equal("Alice", viewModelGroups[0].Members[0].FirstName);
            Assert.Equal("Charlie", viewModelGroups[1].Members[0].FirstName);
        }

        [Fact]
        public void Members_ShouldBeSortedByIsCheckedInAndFirstName()
        {
            // Arrange
            var employees = GetMockEmployees();
            var group = new Group(1, "Test Group");
            group.InitializeMembers(employees);

            // Act
            var sorted = group.Members
                .OrderByDescending(e => e.IsCheckedIn)
                .ThenBy(e => e.FirstName)
                .ToList();

            // Assert
            Assert.Equal("Alice", sorted[0].FirstName);
            Assert.Equal("Charlie", sorted[1].FirstName);
            Assert.Equal("Bob", sorted[2].FirstName);
            Assert.Equal("David", sorted[3].FirstName);
        }

        [Fact]
        public void ChangingIsCheckedIn_ShouldTriggerResorting()
        {
            // Arrange
            var employees = GetMockEmployees();
            var group = new Group(1, "Test Group");
            group.InitializeMembers(employees);

            var bob = group.Members.First(e => e.FirstName == "Bob");
            bob.IsCheckedIn = true;

            // Act
            var sorted = group.Members
                .OrderByDescending(e => e.IsCheckedIn)
                .ThenBy(e => e.FirstName)
                .ToList();

            // Assert
            Assert.Equal("Alice", sorted[0].FirstName);
            Assert.Equal("Bob", sorted[1].FirstName);
            Assert.Equal("Charlie", sorted[2].FirstName);
        }

        [Fact]
        public void BulkUpdates_ShouldResortCorrectly()
        {
            // Arrange
            var employees = GetMockEmployees();
            var group = new Group(1, "Test Group");
            group.InitializeMembers(employees);

            // Act
            foreach (var emp in group.Members)
            {
                emp.IsCheckedIn = true;
            }

            var sorted = group.Members
                .OrderByDescending(e => e.IsCheckedIn)
                .ThenBy(e => e.FirstName)
                .ToList();

            // Assert
            Assert.True(sorted.All(e => e.IsCheckedIn));
            Assert.Equal("Alice", sorted[0].FirstName);
            Assert.Equal("Bob", sorted[1].FirstName);
            Assert.Equal("Charlie", sorted[2].FirstName);
            Assert.Equal("David", sorted[3].FirstName);
        }
    }
}
