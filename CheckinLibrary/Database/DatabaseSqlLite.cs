using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using Dapper;
using CheckinLibrary.Models;
using System.Collections.ObjectModel;
using static CheckinLibrary.Models.Absence;

namespace CheckinLibrary.Database
{
    public class DatabaseSqlLite : IDatabaseHelper
    {
        private readonly string _databasePath;
        private readonly string _connectionString;

        public DatabaseSqlLite(string databasePath = "CheckinLibrary.db")
        {
            _databasePath = databasePath;
            _connectionString = $"Data Source={_databasePath};Version=3;";
        }

        private SQLiteConnection GetConnection()
        {
            return new SQLiteConnection(_connectionString); 
        }

        private void Execute(string sql, object parameters = null)
        {
            using var connection = GetConnection();
            connection.Open();
            connection.Execute(sql, parameters);
        }

        private T QuerySingle<T>(string sql, object parameters = null)
        {
            using var connection = GetConnection();
            connection.Open();
            return connection.QuerySingleOrDefault<T>(sql, parameters);
        }

        private List<T> Query<T>(string sql, object parameters = null)
        {
            using var connection = GetConnection();
            connection.Open();
            return connection.Query<T>(sql, parameters).AsList();
        }

        // 🔹 Card Scanning
        public void CardScanned(string cardID)
        {
            string query = "INSERT INTO cardScanHistory (cardID, scanTime) VALUES (@cardID, CURRENT_TIMESTAMP)";
            Execute(query, new { cardID });
        }

        // 🔹 Admin User Management
        public void CreateUser(string username, string password)
        {
            string query = @"INSERT INTO adminUser (username, hashedPassword) VALUES (@username, @passwordHash)";
            string passwordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(password);
            Execute(query, new { username, passwordHash });
        }

        public AdminUser? Login(string username, string password)
        {
            string hashQuery = "SELECT hashedPassword FROM adminUser WHERE username = @username";
            string? hashedPassword = QuerySingle<string>(hashQuery, new { username });

            if (hashedPassword == null || !BCrypt.Net.BCrypt.EnhancedVerify(password, hashedPassword))
                return null;

            string selectQuery = "SELECT ID, username FROM adminUser WHERE username = @username";
            return QuerySingle<AdminUser>(selectQuery, new { username });
        }

        public List<AdminUser> GetAdminUsers()
        {
            string query = "SELECT * FROM adminUser";
            return Query<AdminUser>(query);
        }

        public void Delete(int ID)
        {
            string query = "DELETE FROM adminUser WHERE ID = @ID";
            Execute(query, new { ID });
        }

        // 🔹 Employee Management
        public List<Employee> GetAllEmployees()
        {
            string query = "SELECT ID, cardID, firstName, middleName, lastName, isOffSite, offSiteUntil FROM employee";
            return Query<Employee>(query);
        }

        public Employee? GetFromCardId(string cardID)
        {
            string query = "SELECT ID, cardID, firstName, middleName, lastName, isOffSite, offSiteUntil FROM employee WHERE cardID = @cardID";
            return QuerySingle<Employee>(query, new { cardID });
        }

        public void UpdateDb(string cardID, string firstName, string middleName, string lastName, bool isOffSite, DateTime? offSiteUntil, int id)
        {
            string query = @"UPDATE employee 
                             SET cardID = @CardID, firstName = @FirstName, middleName = @MiddleName, lastName = @LastName, 
                                 isOffSite = @IsOffSite, offSiteUntil = @OffSiteUntil 
                             WHERE ID = @ID";
            Execute(query, new { CardID = cardID, FirstName = firstName, MiddleName = middleName, LastName = lastName, IsOffSite = isOffSite, OffSiteUntil = offSiteUntil, ID = id });
        }

        public void DeleteFromDb(int ID)
        {
            string query = "DELETE FROM employee WHERE ID = @ID";
            Execute(query, new { ID });
        }

        // 🔹 On-Site Time Tracking
        public (DateTime? ArrivalTime, DateTime? DepartureTime) GetUpdatedSiteTimes(int employeeId)
        {
            string query = "SELECT arrivalTime, departureTime FROM onSiteTime WHERE employeeID = @ID ORDER BY arrivalTime DESC LIMIT 1";
            return QuerySingle<(DateTime?, DateTime?)>(query, new { ID = employeeId });
        }

        public List<OnSiteTime> GetOnsiteTimesForEmployee(Employee employee)
        {
            string query = "SELECT * FROM onSiteTime WHERE employeeID = @employeeID";
            return Query<OnSiteTime>(query, new { employeeID = employee.ID });
        }

        public void UpdateMutipleSiteTimes(List<OnSiteTime> siteTimes)
        {
            string query = @"UPDATE onSiteTime SET arrivalTime = @ArrivalTime, departureTime = @DepartureTime WHERE ID = @Id";
            Execute(query, siteTimes);
        }

        public OnSiteTime AddTimeToDb(int employeeId, DateTime arrivalTime, DateTime? departureTime)
        {
            string query = @"INSERT INTO onSiteTime (employeeID, arrivalTime, departureTime)
                             VALUES (@employeeId, @arrivalTime, @departureTime); 
                             SELECT last_insert_rowid();";
            int siteTimeId = QuerySingle<int>(query, new { employeeId, arrivalTime, departureTime });
            return new OnSiteTime(siteTimeId, employeeId, arrivalTime, departureTime);
        }

        public void DeleteFromDbOnSiteTime(int Id)
        {
            string query = "DELETE FROM onSiteTime WHERE ID = @id";
            Execute(query, new { id = Id });
        }

        // 🔹 Group Management
        public List<Group> GetAllGroups(List<Employee> employees)
        {
            string query = "SELECT * FROM [group]";
            var groups = Query<Group>(query);

            foreach (var group in groups)
            {
                string selectEmployeesQuery = "SELECT employeeID FROM employeeGroup WHERE groupID = @groupID";
                var employeeIDs = Query<int>(selectEmployeesQuery, new { groupID = group.ID });

                group.InitializeMembers(employees.Where(e => employeeIDs.Contains(e.ID)));
            }

            return groups;
        }

        public void RemoveGroupDb(int ID)
        {
            string query = "DELETE FROM [group] WHERE ID = @ID";
            Execute(query, new { ID });
        }

        public string UpdateName(string name, int ID)
        {
            string query = "UPDATE [group] SET name = @name WHERE ID = @ID";
            Execute(query, new { name, ID });
            return name;
        }

        public void UpdateVisibility(bool visibility, bool Isvisible, int ID)
        {
            string query = "UPDATE [group] SET isvisible = @isvisible WHERE ID = @ID";
            Execute(query, new { isvisible = Isvisible, ID });
        }

        public bool AddEmployee(Employee employee, ObservableCollection<Employee> Members, int ID)
        {
            if (Members.Contains(employee))
                return false;

            string query = "INSERT INTO employeeGroup (employeeID, groupID) VALUES (@employeeID, @groupID)";
            Execute(query, new { employeeID = employee.ID, groupID = ID });
            return true;
        }

        public bool RemoveEmployee(Employee employee, ObservableCollection<Employee> Members, int ID)
        {
            if (!Members.Contains(employee))
                return false;

            string query = "DELETE FROM employeeGroup WHERE employeeID = @employeeID AND groupID = @groupID";
            Execute(query, new { employeeID = employee.ID, groupID = ID });
            return true;
        }

        public Group NewGroup(string name)
        {
            string query = "INSERT INTO [group] (name) VALUES (@name); SELECT last_insert_rowid();";
            int newGroupId = QuerySingle<int>(query, new { name });
            return new Group(newGroupId, name);
        }

        //From Absence
        public Absence InsertAbsence(int _employeeId, DateTime _fromDate, DateTime _toDate, string _note, absenceReason _reason)
        {
            string insertQuery = @"
            INSERT INTO Absence (employeeId, fromDate, toDate, note, AbsenceReason)
            VALUES (@employeeId, @fromDate, @toDate, @note, @reason);
            SELECT last_insert_rowid();";

            using var connection = GetConnection();
            if (connection == null)
                throw new Exception("Could not establish database connection!");

            var absenceId = connection.ExecuteScalar<int>(insertQuery, new
            {
                employeeId = _employeeId,
                fromDate = _fromDate,
                toDate = _toDate,
                note = _note,
                reason = (int)_reason
            });

            return new Absence(absenceId, _employeeId, _fromDate, _toDate, _note, _reason);
        }
        public void EditAbsence(List<Absence> absences)
        {
            string editQuery = @"
            UPDATE Absence SET 
            fromDate = @FromDate, 
            toDate = @ToDate, 
            note = @Note, 
            AbsenceReason = @AbsenceReason
            WHERE ID = @ID";

            using var connection = GetConnection();
            if (connection == null)
                throw new Exception("Could not establish database connection!");

            connection.Execute(editQuery, absences);
        }

        public void DeleteAbsence(int _id)
        {
            string deleteQuery = @"DELETE FROM Absence WHERE ID = @id";

            using var connection = GetConnection(); 
            if (connection == null)
                throw new Exception("Could not establish database connection!");

            connection.Execute(deleteQuery, new { id = _id });
        }
        public List<Absence> GetAllAbsence(Employee employee)
        {
            string selectQuery = @"SELECT * FROM Absence WHERE employeeId = @employeeId";

            using var connection = GetConnection();
            if (connection == null)
                throw new Exception("Could not establish database connection!");

            var absences = connection.Query<Absence>(selectQuery, new { employeeId = employee.ID })
            .Select(t =>
            {
                t.AbsenceReason = Enum.TryParse<absenceReason>(t.AbsenceReason.ToString(), out var reason) ? reason : absenceReason.Syg;

                // Ensure that FromTime and ToTime are populated based on FromDate and ToDate
                if (t.FromDate != null)
                {
                    t.FromTime = TimeOnly.FromDateTime(t.FromDate);
                }
                if (t.ToDate != null)
                {
                    t.ToTime = TimeOnly.FromDateTime(t.ToDate);
                }

                return t;
            })
            .ToList();

            return absences;
        }

    }
}
