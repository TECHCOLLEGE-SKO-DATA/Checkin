using CheckInSystem.Models;
using Xunit.Sdk;

namespace CheckInSystem.Tests

{
    public class UnitTest1
    {
        [Fact]
        public void ShortenName()
        {

            Employee emp = new Employee();
            emp.MiddleName = "Test";
            Assert.Equal("T.", emp.MiddleNameShort);
            emp.MiddleName = "Kha-six";
            Assert.Equal("K.", emp.MiddleNameShort);
            emp.MiddleName = "Kda Jar";
            Assert.Equal("K. J.", emp.MiddleNameShort);
            emp.MiddleName = "Kha'zig";
            Assert.Equal("K.", emp.MiddleNameShort);
            emp.MiddleName = "Testing Vervy Long Names";
            Assert.Equal("T. V. L. N.", emp.MiddleNameShort);
            emp.MiddleName = "";
            Assert.Equal("", emp.MiddleNameShort);
        }
        [Fact]
        public void ValidatorAdminUser_Tests()
        {
            AdminUser adminUser = new AdminUser("Kaj");
            string errormessage = Validator.IsAdminUserValid(adminUser);
            Assert.Equal("", errormessage);

            adminUser = new AdminUser("Ka");
            errormessage = Validator.IsAdminUserValid(adminUser);
            Assert.NotEqual("", errormessage);

            string pwd1 = "bøv";
            Assert.Empty(Validator.IsAdminPasswordValid(pwd1));
            string pwd2 = "bø";
            Assert.NotEmpty(Validator.IsAdminPasswordValid(pwd2));

        }
        [Fact]
        public void ValidatorGroup_Tests()
        {
            Group group = new Group();
            group.Name = "Sko";
            string errorMessage = Validator.IsGroupNameValid(group);
            Assert.Equal("", errorMessage);

            group.Name = "Sk";
            errorMessage = Validator.IsGroupNameValid(group);
            Assert.NotEqual("", errorMessage);

            group.Name = "Sko!";
            errorMessage = Validator.IsGroupNameValid(group);
            Assert.Equal("", errorMessage);

            group.Name = "Sk!";
            errorMessage = Validator.IsGroupNameValid(group);
            Assert.NotEqual("", errorMessage);

            group.Name = "sko";
            errorMessage = Validator.IsGroupNameValid(group);
            Assert.NotEqual("", errorMessage);

        }

        [Fact]
        public void ValidatorEmployee_Tests()
        {
            Employee employee = new Employee(1);
            string EmployeeErrorID = Validator.IsEmployeeIdValid(employee);
            Assert.Equal("", EmployeeErrorID);

            employee = new Employee(0);
            EmployeeErrorID = Validator.IsEmployeeIdValid(employee);
            Assert.NotEqual("", EmployeeErrorID);



            employee = new Employee();
            employee.FirstName = "Test";
            employee.LastName = "Test";
            string EmployeeErrorName = Validator.IsEmployeeNameValid(employee);
            Assert.Equal("", EmployeeErrorName);

            employee = new Employee();
            employee.FirstName = "";
            employee.LastName = "";
            EmployeeErrorName = Validator.IsEmployeeNameValid(employee);
            Assert.NotEqual("", EmployeeErrorName);


            var today = new DateOnly();
            var morning = new TimeOnly(7, 15);
            var quittingtime = new TimeOnly(15, 15);

            employee = new Employee();
            employee.ArrivalTime = new DateTime(today, quittingtime);
            employee.DepartureTime = new DateTime(today, morning);
            bool EmployeeErrorTime = Validator.IsEmployeeTimeValid(employee);
            Assert.False(EmployeeErrorTime);

            
            employee = new Employee();
            employee.ArrivalTime = new DateTime(today, morning);            
            employee.DepartureTime = new DateTime(today, quittingtime);
            EmployeeErrorTime = Validator.IsEmployeeTimeValid(employee);
            Assert.True(EmployeeErrorTime);

        }

       
    }
}