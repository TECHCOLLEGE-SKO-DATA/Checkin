using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CheckinLibrary.Models;
using static CheckinLibrary.Models.Absence;

namespace CheckinLibrary.Database
{
    public interface IDatabaseHelper
    {
        // 🔹 Card Scanning
        void CardScanned(string cardID);

        // 🔹 Admin User Management
        void CreateUser(string username, string password);
        AdminUser? Login(string username, string password);
        List<AdminUser> GetAdminUsers();
        void Delete(int ID);

        // 🔹 Employee Management
        List<Employee> GetAllEmployees();
        void UpdateDb(string cardID, string firstName, string middleName, string lastName, bool isOffSite, DateTime? offSiteUntil, int id);
        Employee? GetFromCardId(string cardID);
        void DeleteFromDb(int ID);

        // 🔹 On-Site Time Tracking
        (DateTime? ArrivalTime, DateTime? DepartureTime) GetUpdatedSiteTimes(int employeeId);
        List<OnSiteTime> GetOnsiteTimesForEmployee(Employee employee);
        void UpdateMutipleSiteTimes(List<OnSiteTime> siteTimes);
        OnSiteTime AddTimeToDb(int employeeId, DateTime arrivalTime, DateTime? departureTime);
        void DeleteFromDbOnSiteTime(int Id);

        // 🔹 Group Management
        List<Group> GetAllGroups(List<Employee> employees);
        void RemoveGroupDb(int ID);
        string UpdateName(string name, int ID);
        void UpdateVisibility(bool visibility, bool Isvisible, int ID);
        bool AddEmployee(Employee employee, ObservableCollection<Employee> Members, int ID);
        bool RemoveEmployee(Employee employee, ObservableCollection<Employee> Members, int ID);
        Group NewGroup(string name);

        // 🔹 Absence Management
        Absence InsertAbsence(int employeeId, DateTime fromDate, DateTime toDate, string note, absenceReason reason);
        void EditAbsence(List<Absence> absences);
        void DeleteAbsence(int id);
        List<Absence> GetAllAbsence(Employee employee);
    }
}
