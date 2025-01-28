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
                new Employee { ID = 1, FirstName = "Alice", IsCheckedIn = true },
                new Employee { ID = 2, FirstName = "Bob", IsCheckedIn = false },
                new Employee { ID = 3, FirstName = "Charlie", IsCheckedIn = true },
                new Employee { ID = 4, FirstName = "David", IsCheckedIn = false },
            };
        }

        public static List<Group> GetMockGroups(List<Employee> employees)
        {
            var group1 = new Group { ID = 1, Name = "Group 1" };
            group1.Members = new ObservableCollection<Employee>(employees.Take(2)); // Alice, Bob

            var group2 = new Group { ID = 2, Name = "Group 2" };
            group2.Members = new ObservableCollection<Employee>(employees.Skip(2)); // Charlie, David

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
            var group = new Group
            {
                ID = 1,
                Name = "Test Group",
                Members = new ObservableCollection<Employee>(employees)
            };

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
            var group = new Group
            {
                ID = 1,
                Name = "Test Group",
                Members = new ObservableCollection<Employee>(employees)
            };

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
            var group = new Group
            {
                ID = 1,
                Name = "Test Group",
                Members = new ObservableCollection<Employee>(employees)
            };

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
