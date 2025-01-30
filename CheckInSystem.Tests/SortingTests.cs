using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using Xunit;
using CheckInSystem.Models;

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
            Assert.Equal(2, viewModelGroups.Count); // Ensure 2 groups are loaded
            Assert.Equal(2, viewModelGroups[0].Members.Count); // Group 1 has 2 members
            Assert.Equal(2, viewModelGroups[1].Members.Count); // Group 2 has 2 members
            Assert.Equal("Alice", viewModelGroups[0].Members[0].FirstName); // First member in Group 1 is Alice
            Assert.Equal("Charlie", viewModelGroups[1].Members[0].FirstName); // First member in Group 2 is Charlie
        }

        [Fact]
        public void Members_ShouldBeSortedByIsCheckedInAndFirstName()
        {
            // Arrange
            var employees = GetMockEmployees();
            var group = new Group(1, "Test Group");
            group.InitializeMembers(employees);

            // Act
            var view = CollectionViewSource.GetDefaultView(group.Members);
            view.SortDescriptions.Add(new SortDescription(nameof(Employee.IsCheckedIn), ListSortDirection.Descending));
            view.SortDescriptions.Add(new SortDescription(nameof(Employee.FirstName), ListSortDirection.Ascending));
            view.Refresh();

            // Assert
            Assert.Equal("Alice", ((Employee)view.Cast<Employee>().First()).FirstName); // Checked-in, alphabetical
            Assert.Equal("Charlie", ((Employee)view.Cast<Employee>().Skip(1).First()).FirstName); // Checked-in, alphabetical
            Assert.Equal("Bob", ((Employee)view.Cast<Employee>().Skip(2).First()).FirstName); // Not checked-in
        }

        [Fact]
        public void ChangingIsCheckedIn_ShouldTriggerResorting()
        {
            // Arrange
            var employees = GetMockEmployees();
            var group = new Group(1, "Test Group");
            group.InitializeMembers(employees);

            var view = CollectionViewSource.GetDefaultView(group.Members);
            view.SortDescriptions.Add(new SortDescription(nameof(Employee.IsCheckedIn), ListSortDirection.Descending));
            view.SortDescriptions.Add(new SortDescription(nameof(Employee.FirstName), ListSortDirection.Ascending));
            view.Refresh();

            // Act
            var bob = group.Members.First(e => e.FirstName == "Bob");
            bob.IsCheckedIn = true; // Update property
            view.Refresh(); // Refresh sorting

            // Assert
            Assert.Equal("Bob", ((Employee)view.Cast<Employee>().Skip(1).First()).FirstName); // Bob should now be sorted with checked-in members
        }

        [Fact]
        public void BulkUpdates_ShouldResortCorrectly()
        {
            // Arrange
            var employees = GetMockEmployees();
            var group = new Group(1, "Test Group");
            group.InitializeMembers(employees);

            var view = CollectionViewSource.GetDefaultView(group.Members);
            view.SortDescriptions.Add(new SortDescription(nameof(Employee.IsCheckedIn), ListSortDirection.Descending));
            view.SortDescriptions.Add(new SortDescription(nameof(Employee.FirstName), ListSortDirection.Ascending));
            view.Refresh();

            // Act
            foreach (var employee in group.Members)
            {
                employee.IsCheckedIn = true; // Set all to checked-in
            }
            view.Refresh();

            // Assert
            Assert.True(group.Members.All(e => e.IsCheckedIn)); // Ensure all are checked in
            Assert.Equal("Alice", ((Employee)view.Cast<Employee>().First()).FirstName); // Alphabetical order within checked-in
        }
    }
}
