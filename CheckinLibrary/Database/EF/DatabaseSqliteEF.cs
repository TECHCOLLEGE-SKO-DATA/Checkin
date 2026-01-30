using CheckinLibrary.Background_tasks;
using CheckinLibrary.Models;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CheckinLibrary.Database.EF;

namespace CheckinLibrary.Database
{
    public class DatabaseSqliteEF : IDatabaseHelper
    {
        private readonly CheckInDbContext _db;

        public DatabaseSqliteEF(CheckInDbContext db)
        {
            _db = db;
        }

        // -------------------------------------------------
        // CardScanned (no stored procedure)
        // -------------------------------------------------
        public void CardScanned(string cardID)
        {
            var employee = _db.Employees
                .Include(e => e.OnSiteTimes)
                .FirstOrDefault(e => e.CardID == cardID);

            if (employee == null)
            {
                employee = new EmployeeDTO { CardID = cardID, FirstName = "Unknown", LastName = "Unknown" };
                _db.Employees.Add(employee);
                _db.SaveChanges();
                return;
            }

            var lastTime = employee.OnSiteTimes
                .OrderByDescending(t => t.ArrivalTime)
                .FirstOrDefault();

            if (lastTime == null || lastTime.DepartureTime != null)
            {
                _db.OnSiteTimes.Add(new OnSiteTimeDTO
                {
                    EmployeeID = employee.ID,
                    ArrivalTime = DateTime.Now
                });
            }
            else
            {

                lastTime.DepartureTime = DateTime.Now;
            }

            _db.SaveChanges();
        }

        // -------------------------------------------------
        // Admin Users
        // -------------------------------------------------
        public void CreateUser(string username, string password)
        {
            var passwordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(password);
            var user = new AdminUserDTO
            {
                Username = username,
                HashedPassword = passwordHash
            };
            _db.AdminUsers.Add(user);
            _db.SaveChanges();
        }

        public void UpdateUser(string username, string password, int id)
        {
            var user = _db.AdminUsers.Find(id);
            if (user == null) return;

            user.Username = username;
            user.HashedPassword = BCrypt.Net.BCrypt.EnhancedHashPassword(password);
            _db.SaveChanges();
        }

        public AdminUser? Login(string username, string password)
        {
            var userDto = _db.AdminUsers.FirstOrDefault(u => u.Username == username);
            if (userDto == null) return null;

            if (!BCrypt.Net.BCrypt.EnhancedVerify(password, userDto.HashedPassword))
                return null;

            return userDto.Adapt<AdminUser>();
        }

        public List<AdminUser> GetAdminUsers()
        {
            return _db.AdminUsers
                .AsNoTracking()
                .Select(u => new AdminUser(u.Username))
                .ToList();
        }

        public void Delete(int id)
        {
            var user = _db.AdminUsers.Find(id);
            if (user == null) return;

            _db.AdminUsers.Remove(user);
            _db.SaveChanges();
        }

        // -------------------------------------------------
        // Employees
        // -------------------------------------------------
        public List<Employee> GetAllEmployees()
        {
            var employees = _db.Employees
                .Include(e => e.OnSiteTimes)
                .AsNoTracking()
                .ToList();

            return employees.Adapt<List<Employee>>();
        }

        public Employee? GetFromCardId(string cardID)
        {
            var employee = _db.Employees
                .Include(e => e.OnSiteTimes)
                .FirstOrDefault(e => e.CardID == cardID);

            return employee?.Adapt<Employee>();
        }

        public void UpdateDb(
            string cardID,
            string firstName,
            string middleName,
            string lastName,
            bool isOffSite,
            DateTime? offSiteUntil,
            int id)
        {
            var emp = _db.Employees.Find(id);
            if (emp == null) return;

            emp.CardID = cardID;
            emp.FirstName = firstName;
            emp.MiddleName = middleName;
            emp.LastName = lastName;
            emp.IsOffSite = isOffSite;
            emp.OffSiteUntil = offSiteUntil;

            _db.SaveChanges();
        }

        public void DeleteFromDb(int id)
        {
            var emp = _db.Employees.Find(id);
            if (emp == null) return;

            _db.Employees.Remove(emp);
            _db.SaveChanges();
        }

        // -------------------------------------------------
        // Groups
        // -------------------------------------------------
        public List<Group> GetAllGroups(List<Employee> employees)
        {
            var groups = _db.Groups
                .Include(g => g.EmployeeGroups)
                    .ThenInclude(eg => eg.Employee)
                        .ThenInclude(e => e.OnSiteTimes)
                .AsNoTracking()
                .ToList();

            var localConfig = TypeAdapterConfig.GlobalSettings.Clone();
            localConfig.RequireExplicitMapping = false;

            return groups.Adapt<List<Group>>(localConfig);
        }

        public void RemoveGroupDb(int id)
        {
            var group = _db.Groups.Find(id);
            if (group == null) return;

            _db.Groups.Remove(group);
            _db.SaveChanges();
        }

        public string UpdateName(string name, int id)
        {
            var group = _db.Groups.Find(id);
            if (group == null) return name;

            group.Name = name;
            _db.SaveChanges();
            return name;
        }

        public void UpdateVisibility(bool visibility, bool isvisible, int id)
        {
            var group = _db.Groups.Find(id);
            if (group == null) return;

            group.Isvisible = isvisible;
            _db.SaveChanges();
        }

        public bool AddEmployee(Employee employee, ObservableCollection<Employee> members, int groupId)
        {
            if (members.Contains(employee)) return false;

            _db.EmployeeGroups.Add(new EmployeeGroupDTO
            {
                EmployeeID = employee.ID,
                GroupID = groupId
            });

            _db.SaveChanges();
            return true;
        }

        public bool RemoveEmployee(Employee employee, ObservableCollection<Employee> members, int groupId)
        {
            var eg = _db.EmployeeGroups
                .FirstOrDefault(e => e.EmployeeID == employee.ID && e.GroupID == groupId);

            if (eg == null) return false;

            _db.EmployeeGroups.Remove(eg);
            _db.SaveChanges();
            return true;
        }

        public Group NewGroup(string name)
        {
            var dto = new GroupDTO { Name = name };
            _db.Groups.Add(dto);
            _db.SaveChanges();

            var group = new Group();
            group.UpdateName(name, dto.ID);
            return group;
        }

        // -------------------------------------------------
        // OnSiteTime
        // -------------------------------------------------
        public List<OnSiteTime> GetOnsiteTimesForEmployee(Employee employee)
        {
            return _db.OnSiteTimes
                .Where(t => t.EmployeeID == employee.ID)
                .AsNoTracking()
                .Adapt<List<OnSiteTime>>();
        }

        public (DateTime? ArrivalTime, DateTime? DepartureTime) GetUpdatedSiteTimes(int employeeId)
        {
            var time = _db.OnSiteTimes
                .Where(t => t.EmployeeID == employeeId)
                .OrderByDescending(t => t.ArrivalTime)
                .FirstOrDefault();

            return (time?.ArrivalTime, time?.DepartureTime);
        }

        public void DeleteFromDbOnSiteTime(int id)
        {
            var time = _db.OnSiteTimes.Find(id);
            if (time == null) return;

            _db.OnSiteTimes.Remove(time);
            _db.SaveChanges();
        }

        public void UpdateMutipleSiteTimes(List<OnSiteTime> siteTimes)
        {
            foreach (var t in siteTimes)
            {
                var dto = _db.OnSiteTimes.Find(t.Id);
                if (dto == null) continue;

                dto.ArrivalTime = t.ArrivalTime ?? DateTime.Now;
                dto.DepartureTime = t.DepartureTime;
            }

            _db.SaveChanges();
        }

        public OnSiteTime AddTimeToDb(int employeeId, DateTime arrivalTime, DateTime? departureTime)
        {
            var dto = new OnSiteTimeDTO
            {
                EmployeeID = employeeId,
                ArrivalTime = arrivalTime,
                DepartureTime = departureTime
            };

            _db.OnSiteTimes.Add(dto);
            _db.SaveChanges();

            return dto.Adapt<OnSiteTime>();
        }

        // -------------------------------------------------
        // Absence
        // -------------------------------------------------
        public Absence InsertAbsence(int employeeId, DateTime fromDate, DateTime toDate, string note, int reasonId)
        {
            var dto = new AbsenceDTO
            {
                EmployeeId = employeeId,
                FromDate = fromDate,
                ToDate = toDate,
                Note = note,
                AbsenceReasonId = reasonId
            };

            _db.Absences.Add(dto);
            _db.SaveChanges();

            new AbsencBackGroundService().AbsenceTask();

            return dto.Adapt<Absence>();
        }

        public void EditAbsence(List<Absence> absences)
        {
            foreach (var a in absences)
            {
                var dto = _db.Absences.Find(a.ID);
                if (dto == null) continue;

                dto.FromDate = a.FromDate;
                dto.ToDate = a.ToDate;
                dto.Note = a.Note;
                dto.AbsenceReasonId = a.AbsenceReasonId;
            }

            _db.SaveChanges();
        }

        public void DeleteAbsence(int id)
        {
            var dto = _db.Absences.Find(id);
            if (dto == null) return;

            _db.Absences.Remove(dto);
            _db.SaveChanges();
        }

        public List<Absence> GetAllAbsence(Employee employee)
        {
            return _db.Absences
                .Where(a => a.EmployeeId == employee.ID)
                .AsNoTracking()
                .Adapt<List<Absence>>();
        }
    }
}
