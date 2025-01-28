namespace CheckInSystem.Database;

using CheckInSystem.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;
using System.Collections.ObjectModel;
using CheckInSystem.CardReader;

public class DatabaseHelper
{
    //From ACR122U CardScanned

    public void CardScanned(string cardID)
    {
        string insertQuery = "EXEC CardScanned @cardID";

        using var connection = Database.GetConnection();
        if (connection == null)
            throw new Exception("Could not establish database connection!");

        connection.Query(insertQuery, new { cardID = cardID });
    }

    //From ACR122U CardScanned

    //From Admin User
    public static void CreateUser(string username, string password)
    {
        string insertQuery = @"INSERT INTO adminUser (username, hashedPassword) VALUES (@username, @passwordHash)";

        string passwordHash = BCrypt.EnhancedHashPassword(password);
        Debug.WriteLine(passwordHash);

        using var connection = Database.GetConnection();
        if (connection == null)
            throw new Exception("Could not establish database connection!");

        connection.Query(insertQuery, new { username = username, passwordHash = passwordHash });
    }

    public static AdminUser? Login(string username, string password)
    {
        string passwordHashQuery = @"SELECT hashedPassword FROM adminUser WHERE username = @username";
        string selectQuery = @"SELECT ID, username FROM adminUser WHERE username = @username";

        using var connection = Database.GetConnection();
        if (connection == null)
            throw new Exception("Could not establish database connection!");

        string? hashedPassword = connection.Query<string>(passwordHashQuery, new { username = username }).FirstOrDefault();
        if (hashedPassword == null) return null;
        if (!BCrypt.EnhancedVerify(password, hashedPassword)) return null;

        var adminUser = connection.Query<AdminUser>(selectQuery, new { username = username }).FirstOrDefault();
        return adminUser;
    }

    public static List<AdminUser> GetAdminUsers()
    {
        string selectQuery = @"SELECT * FROM adminUser";

        using var connection = Database.GetConnection();
        if (connection == null)
            throw new Exception("Could not establish database connection!");

        var adminUsers = connection.Query<AdminUser>(selectQuery).ToList();
        return adminUsers;
    }

    public static void Delete(int ID)
    {
        string deletionQuery = @"DELETE FROM adminUser WHERE ID = @id";

        using var connection = Database.GetConnection();
        if (connection == null)
            throw new Exception("Could not establish database connection!");

        connection.Query(deletionQuery, new { id = ID });
    }

    //From Admin User

    //From Employee
    public static List<Employee> GetAllEmployees()
    {
        string selectQuery = @"SELECT employee.ID, cardid, firstname, middlename, lastname, isoffsite, offsiteuntil, arrivaltime, departuretime,
            [dbo].[IsEmployeeCheckedIn](employee.ID) as IsCheckedIn
            FROM employee
            LEFT JOIN dbo.onSiteTime on onSiteTime.ID = (
            SELECT TOP (1) ID 
            FROM OnSiteTime
            WHERE employee.ID = OnSiteTime.employeeID
            ORDER BY arrivalTime DESC)";

        using var connection = Database.GetConnection();
        if (connection == null)
            throw new Exception("Could not establish database connection!");

        var employees = connection.Query<Employee>(selectQuery).ToList();
        return employees;
    }

    public void UpdateDb(string cardID, string firstName, string middleName, string lastName, bool isOffSite, DateTime? offSiteUntil, int id)
    {
        string updateQuery = @"
        UPDATE employee
        SET cardID = @CardID,
        firstName = @FirstName,
        middleName = @MiddleName,
        lastName = @LastName,
        isOffSite = @IsOffSite,
        offSiteUntil = @OffSiteUntil
        WHERE ID = @id";

        using var connection = Database.GetConnection();
        if (connection == null)
            throw new Exception("Could not establish database connection!");

        connection.Query(updateQuery, new { CardID = cardID, FirstName = firstName, MiddleName = middleName, LastName = lastName, IsOffSite = isOffSite, OffSiteUntil = offSiteUntil, ID = id });
    }


    public static Employee? GetFromCardId(string cardID)
    {
        string selectQuery = @"SELECT employee.ID, cardid, firstname, middlename, lastname, isoffsite, offsiteuntil, arrivaltime, departuretime,
            [dbo].[IsEmployeeCheckedIn](employee.ID) as IsCheckedIn
            FROM employee
            LEFT JOIN dbo.onSiteTime on onSiteTime.ID = (
            SELECT TOP (1) ID 
            FROM OnSiteTime
            WHERE employee.ID = OnSiteTime.employeeID
            ORDER BY arrivalTime DESC)
            WHERE cardID = @cardID";

        using var connection = Database.GetConnection();
        if (connection == null)
            throw new Exception("Could not establish database connection!");

        var employees = connection.Query<Employee>(selectQuery, new { cardID = cardID }).FirstOrDefault();
        return employees;
    }

    public (DateTime? ArrivalTime, DateTime? DepartureTime) GetUpdatedSiteTimes(int employeeId)
    {
        DateTime? ArrivalTime = null;
        DateTime? DepartureTime = null;
        string selectQuery = @"Select TOP(1) * FROM onSiteTime
                WHERE employeeID = @ID
                ORDER BY arrivalTime desc";

        try
        {
            using var connection = Database.GetConnection();
            if (connection == null)
                throw new Exception("Could not establish database connection!");

            var siteTime = connection.QuerySingle<OnSiteTime>(selectQuery, new { ID = employeeId });

            ArrivalTime = siteTime.ArrivalTime;
            DepartureTime = siteTime.DepartureTime;
        }
        catch (Exception)
        {
            ArrivalTime = null;
            DepartureTime = null;
        }

        return (ArrivalTime, DepartureTime);
    }
    
    public static void DeleteFromDb(int Id)
    {
        string deletionQuery = @"DELETE employee WHERE ID = @ID";

        using var connection = Database.GetConnection();
        if (connection == null)
            throw new Exception("Could not establish database connection!");

        connection.Query(deletionQuery, Id);
    }

    //From Employee

    //From Group
    public void RemoveGroupDb(int ID)
    {
        string deletionQuery = @"DELETE [group] WHERE ID = @ID";

        using var connection = Database.GetConnection();
        if (connection == null)
            throw new Exception("Could not establish database connection!");

        connection.Query(deletionQuery, new { ID = ID });
    }

    public string UpdateName(string name, int ID)
    {
        string updateQuery = @"UPDATE [group] 
            SET name = @name
            WHERE ID = @ID";

        using var connection = Database.GetConnection();
        if (connection == null)
            throw new Exception("Could not establish database connection!");

        connection.Query(updateQuery, new { name = name, ID = ID });

        return name;
    }

    public void Updatevisibility(bool visibility, bool Isvisible, int ID)
    {
        string updateQuery = @"UPDATE [group] 
            SET isvisible = @isvisible
            WHERE ID = @ID";

        using var connection = Database.GetConnection();
        if (connection == null)
            throw new Exception("Could not establish database connection!");

        connection.Query(updateQuery, new { isvisible = Isvisible, ID = ID });
    }

    public void AddEmployee(Employee employee,ObservableCollection<Employee> Members, int ID)
    {
        if (Members.Contains(employee)) return;

        string insertQuery = @"INSERT INTO employeeGroup (employeeID, groupID) VALUES (@employeeID, @groupID)";

        using var connection = Database.GetConnection();
        if (connection == null)
            throw new Exception("Could not establish database connection!");

        connection.Query(insertQuery, new { employeeID = employee.ID, @groupID = ID });

        Members.Add(employee);
    }

    public void RemoveEmployee(Employee employee, ObservableCollection<Employee> Members, int ID)
    {
        if (!Members.Contains(employee)) return;

        string deletionQuery = @"DELETE employeeGroup WHERE employeeID = @employeeID AND groupID = @groupID";

        using var connection = Database.GetConnection();
        if (connection == null)
            throw new Exception("Could not establish database connection!");

        connection.Query(deletionQuery, new { employeeID = employee.ID, @groupID = ID });

        Members.Remove(employee);
    }

    public static List<Group> GetAllGroups(List<Employee> employees)
    {
        string selectQueryGroups = @"SELECT * FROM [group]";
        string selectQueryEmployeesInGroup = @"SELECT employeeID FROM employeeGroup WHERE groupID = {0}";

        using var connection = Database.GetConnection();
        if (connection == null)
            throw new Exception("Could not establish database connection!");

        var groups = connection.Query<Group>(selectQueryGroups).ToList();

        foreach (var group in groups)
        {
            string formattedQuery = string.Format(selectQueryEmployeesInGroup, group.ID);
            var employeeIDs = connection.Query<int>(formattedQuery).ToList();
            group.Members = new ObservableCollection<Employee>();

            foreach (var employeeID in employeeIDs)
            {
                var temp = employees.Where(i => i.ID == employeeID).FirstOrDefault();
                if (temp != null)
                {
                    group.Members.Add(temp);
                }
            }
        }

        return groups;
    }
    public static Group NewGroup(String name)
    {
        string insertQuery = @"INSERT INTO [group] (name) VALUES (@name)
            SELECT SCOPE_IDENTITY();";
        Group newGroup = new Group()
        {
            Name = name,
            Members = new ObservableCollection<Employee>()
        };

        using var connection = Database.GetConnection();
        if (connection == null)
            throw new Exception("Could not establish database connection!");

        newGroup.ID = connection.QueryFirst<int>(insertQuery, new { name = name });

        return newGroup;
    }

    //From Group
}
