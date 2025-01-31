using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using Dapper;
using CheckInSystem.Models;
using System.Collections.ObjectModel;

namespace CheckInSystem.Database
{
    public static class DatabaseSqlLite
    {
        private static string _databasePath = "CheckInSystem.db";
        private static string _connectionString = $"Data Source={_databasePath};Version=3;";

        public static SQLiteConnection GetConnection()
        {
            return new SQLiteConnection(_connectionString);
        }

        public static void Execute(string sql, object parameters = null)
        {
            using var connection = GetConnection();
            connection.Open();
            connection.Execute(sql, parameters);
        }

        public static T QuerySingle<T>(string sql, object parameters = null)
        {
            using var connection = GetConnection();
            connection.Open();
            return connection.QuerySingle<T>(sql, parameters);
        }

        public static List<T> Query<T>(string sql, object parameters = null)
        {
            using var connection = GetConnection();
            connection.Open();
            return connection.Query<T>(sql, parameters).AsList();
        }

        // Methods matching DatabaseHelper
        public static List<Employee> GetAllEmployees()
        {
            string selectQuery = @"SELECT ID, cardID, firstName, middleName, lastName, isOffSite, offSiteUntil FROM employee";
            return Query<Employee>(selectQuery);
        }

        public static Employee? GetFromCardId(string cardID)
        {
            string selectQuery = @"SELECT ID, cardID, firstName, middleName, lastName, isOffSite, offSiteUntil FROM employee WHERE cardID = @cardID";
            return QuerySingle<Employee>(selectQuery, new { cardID });
        }

        public static void CreateUser(string username, string passwordHash)
        {
            string insertQuery = @"INSERT INTO adminUser (username, hashedPassword) VALUES (@username, @passwordHash)";
            Execute(insertQuery, new { username, passwordHash });
        }

        public static List<AdminUser> GetAdminUsers()
        {
            string selectQuery = @"SELECT * FROM adminUser";
            return Query<AdminUser>(selectQuery);
        }

        public static void DeleteUser(int ID)
        {
            string deletionQuery = @"DELETE FROM adminUser WHERE ID = @ID";
            Execute(deletionQuery, new { ID });
        }

        public static void UpdateEmployee(Employee employee)
        {
            string updateQuery = @"UPDATE employee SET cardID = @CardID, firstName = @FirstName, middleName = @MiddleName, lastName = @LastName, isOffSite = @IsOffSite, offSiteUntil = @OffSiteUntil WHERE ID = @ID";
            Execute(updateQuery, employee);
        }

        public static void DeleteEmployee(int ID)
        {
            string deletionQuery = @"DELETE FROM employee WHERE ID = @ID";
            Execute(deletionQuery, new { ID });
        }

        public static List<Group> GetAllGroups(List<Employee> employees)
        {
            string selectQueryGroups = "SELECT * FROM [group]";
            var groups = Query<Group>(selectQueryGroups);

            foreach (var group in groups)
            {
                string selectEmployeesQuery = "SELECT employeeID FROM employeeGroup WHERE groupID = @groupID";
                var employeeIDs = Query<int>(selectEmployeesQuery, new { groupID = group.ID });

                group.InitializeMembers(employees.Where(e => employeeIDs.Contains(e.ID)));
            }

            return groups;
        }

        public static void AddEmployeeToGroup(int employeeID, int groupID)
        {
            string insertQuery = @"INSERT INTO employeeGroup (employeeID, groupID) VALUES (@employeeID, @groupID)";
            Execute(insertQuery, new { employeeID, groupID });
        }

        public static void RemoveEmployeeFromGroup(int employeeID, int groupID)
        {
            string deleteQuery = @"DELETE FROM employeeGroup WHERE employeeID = @employeeID AND groupID = @groupID";
            Execute(deleteQuery, new { employeeID, groupID });
        }
    }
}
