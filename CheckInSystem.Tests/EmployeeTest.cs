using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CheckinLibrary.Database;
using CheckinLibrary.Models;
using CheckInSystem.Platform;

namespace CheckInSystem.Tests
{
    public class EmployeeTest
    {
        Group groupModel = new();
        ObservableCollection<Employee>Members {  get; set; }

        //these test aren't gonna work with this not that they did to begin with
        IPlatform platform;

        [Fact]
        public void Employee_Creat_Update()
        {
            platform.Database.CardScanned("abc123die24");

            Employee employee = platform.Database.GetFromCardId("abc123die24");
            Assert.NotNull(employee);

            platform.Database.UpdateDb("abc123die24", "Jhon", "Simon", "Doe", false, DateTime.Now, employee.ID);
        }

        [Fact]
        public void Employee_Get() 
        {
            Employee employee = platform.Database.GetFromCardId("abc123die24");
            Assert.NotNull(employee);

            Assert.Equal("Jhon", employee.FirstName);
            Assert.Equal("abc123die24", employee.CardID);

            platform.Database.CardScanned(employee.CardID);

            List<Employee> employees = platform.Database.GetAllEmployees();
            Assert.Contains(employees, e => e.ID == employee.ID);

            var (arrivalTime, departureTime) = platform.Database.GetUpdatedSiteTimes(employee.ID);

            Assert.NotNull(arrivalTime);
            if (departureTime != null)
            {
                Assert.True(departureTime > arrivalTime);
            }
        }

        [Fact]
        public void Employee_Delete() 
        {
            Employee employee = platform.Database.GetFromCardId("abc123die24");
            Assert.NotNull(employee);

            platform.Database.DeleteFromDb(employee.ID);

            List<Employee> employeesAfterDelete = platform.Database.GetAllEmployees();
            Assert.DoesNotContain(employeesAfterDelete, e => e.ID == employee.ID);
        }
    }
}
