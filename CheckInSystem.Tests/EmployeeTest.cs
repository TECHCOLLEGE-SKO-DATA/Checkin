using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CheckInSystem.Database;
using CheckInSystem.Models;

namespace CheckInSystem.Tests
{
    public class EmployeeTest
    {
        DatabaseHelper databaseHelper = new();
        Group groupModel = new();
        ObservableCollection<Employee>Members {  get; set; }

        [Fact]
        public void Employee_Creat_Update()
        {
            databaseHelper.CardScanned("abc123die24");

            Employee employee = DatabaseHelper.GetFromCardId("abc123die24");
            Assert.NotNull(employee);

            databaseHelper.UpdateDb("abc123die24", "Jhon", "Simon", "Doe", false, DateTime.Now, employee.ID);
        }

        [Fact]
        public void Employee_Get() 
        {
            Employee employee = DatabaseHelper.GetFromCardId("abc123die24");
            Assert.NotNull(employee);

            Assert.Equal("Jhon", employee.FirstName);
            Assert.Equal("abc123die24", employee.CardID);

            databaseHelper.CardScanned(employee.CardID);

            List<Employee> employees = DatabaseHelper.GetAllEmployees();
            Assert.Contains(employees, e => e.ID == employee.ID);

            var (arrivalTime, departureTime) = databaseHelper.GetUpdatedSiteTimes(employee.ID);

            Assert.NotNull(arrivalTime);
            if (departureTime != null)
            {
                Assert.True(departureTime > arrivalTime);
            }
        }

        [Fact]
        public void Employee_Delete() 
        {
            Employee employee = DatabaseHelper.GetFromCardId("abc123die24");
            Assert.NotNull(employee);

            databaseHelper.DeleteFromDb(employee.ID);

            List<Employee> employeesAfterDelete = DatabaseHelper.GetAllEmployees();
            Assert.DoesNotContain(employeesAfterDelete, e => e.ID == employee.ID);
        }
    }
}
