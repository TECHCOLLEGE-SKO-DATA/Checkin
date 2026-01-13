using CheckinLibrary.Background_tasks;
using CheckinLibrary.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckinLibrary.Database
{
    public class DatabaseSqlExpressEf : IDatabaseHelper
    {
        /*
         * ############################################################ *
         * ## this holds quries specifik for SQL Server and exspress ## *
         * ############################################################ *
         */

        //From ACR122U CardScanned

        public void CardScanned(string cardID)
        {
            
        }

        //From ACR122U CardScanned

        //From Admin User
        public void CreateUser(string username, string password)
        {
            
        }

        public void UpdateUser(string username, string password, int Id)
        {

        }

        public AdminUser? Login(string username, string password)
        {
            
        }

        public List<AdminUser> GetAdminUsers()
        {
            
        }

        public void Delete(int ID)
        {
            
        }

        //From Admin User

        //From Employee
        public List<Employee> GetAllEmployees()
        {
            
        }

        public void UpdateDb(string cardID, string firstName, string middleName, string lastName, bool isOffSite, DateTime? offSiteUntil, int id)
        {
            
        }

        public Employee? GetFromCardId(string cardID)
        {
            
        }

        public void DeleteFromDb(int ID)
        {
            
        }


        //From Employee

        //From Group
        public void RemoveGroupDb(int ID)
        {
            
        }

        public string UpdateName(string name, int ID)
        {
            
        }

        public void UpdateVisibility(bool visibility, bool Isvisible, int ID)
        {
            
        }

        public bool AddEmployee(Employee employee, ObservableCollection<Employee> Members, int ID)
        {
            
        }

        public bool RemoveEmployee(Employee employee, ObservableCollection<Employee> Members, int ID)
        {
            
        }

        public List<Group> GetAllGroups(List<Employee> employees)
        {
            
        }



        public Group NewGroup(string name)
        {
            
        }

        //From Group

        //From OnSiteTime
        public List<OnSiteTime> GetOnsiteTimesForEmployee(Employee employee)
        {
            
        }
        public (DateTime? ArrivalTime, DateTime? DepartureTime) GetUpdatedSiteTimes(int employeeId)
        {
            
        }

        public void DeleteFromDbOnSiteTime(int Id)
        {
            
        }

        public void UpdateMutipleSiteTimes(List<OnSiteTime> siteTimes)
        {
            
        }

        public OnSiteTime AddTimeToDb(int employeeId, DateTime arrivalTime, DateTime? departureTime)
        {
            
        }

        //From OnSiteTime

        //From Absence
        public Absence InsertAbsence(int _employeeId, DateTime _fromDate, DateTime _toDate, string _note, int _reasonId)
        {
            
        }

        public void EditAbsence(List<Absence> absences)
        {
            
        }

        public void DeleteAbsence(int _id)
        {
            
        }

        public List<Absence> GetAllAbsence(Employee employee)
        {
            
        }
    }
}
