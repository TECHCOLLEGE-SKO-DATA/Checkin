using CheckInSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckInSystem
{
    public static class Validator
    {
        public static string IsAdminUserValid(AdminUser obj)
        {
            string message = "";
            if (obj.Username.Length < 3)
                message = "Username needs to be longer then 3";
            return message;
        }

        public static string IsAdminPasswordValid(string password)
        {
            string message = "";
            if (password.Length < 3)
                message = "Password needs to be longer then 3";
            return message;
        }

        public static string IsGroupNameValid(Group obj)
        {
            string message = "";
            if (obj.Name.Length < 3)
            {
                return "GroupName must be at least 3 characters long.";
            }
            
            if (!char.IsUpper(obj.Name[0]))
            {
                return "The first charcter must be an uppercase letter.";
            }

            if (!char.IsLetter(obj.Name[1]) || !char.IsLetter(obj.Name[2]))
            {
                return "The second and third characters must be letters.";
            }

            return message;
        }

        public static string IsEmployeeIdValid(Employee employee) 
        {

            string employeeiderror = "";
            
            if(employee.ID == 0)
            {
                employeeiderror = "Employeeid cant be zero.";
            }
            return employeeiderror;
        }
        public static string IsEmployeeNameValid(Employee employeename) 
        {
            string employeenameerror = "";
            if (employeename.FirstName.Length == 0)
            {
                return "Employee first name cant be empty.";
            }
            if (employeename.LastName.Length == 0)
            {
                return "Employee last name cant be empty.";
            }
            return employeenameerror;
        }
        public static bool IsEmployeeTimeValid(Employee employeeTime)
        {
            return employeeTime.ArrivalTime < employeeTime.DepartureTime;
        }
    }
}
