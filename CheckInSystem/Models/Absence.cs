using CheckInSystem.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckInSystem.Models
{
    public class Absence
    {
        public enum absenceReason
        {
            Sick,
            School,
            Vacation
        }

        private DatabaseHelper dbHelper = new();

        private DateTime _oldFromDate;
        private DateTime _oldToDate;
        private string _oldNote;
        private absenceReason _oldReason;

        public TimeOnly FromTime { get; set; }
        public TimeOnly ToTime { get; set; }

        public int ID { get; set; }
        public int EmployeeId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public absenceReason AbsenceReason { get; set; } 
        public string? Note { get; set; } 

        
        public Absence(Absence absence)
        {
            ID = absence.ID;
            EmployeeId = absence.EmployeeId;
            FromDate = absence.FromDate;
            ToDate = absence.ToDate;
            Note = absence.Note;
            AbsenceReason = absence.AbsenceReason;
            
            _oldFromDate = absence.FromDate;
            _oldToDate = absence.ToDate;
            _oldNote = absence.Note;
            _oldReason = absence.AbsenceReason;
        }
        public Absence(int id, int employeeId, DateTime fromDate, DateTime toDate, string note, absenceReason reason)
        {
            ID = id;
            EmployeeId = employeeId;

            FromDate = fromDate;
            ToDate = toDate;
            Note = note;
            AbsenceReason = reason;

            FromTime = TimeOnly.FromDateTime(fromDate);
            ToTime = TimeOnly.FromDateTime(toDate);

            _oldFromDate = fromDate;
            _oldToDate = toDate;
            _oldNote = note;
            _oldReason = reason;
        }

        public Absence InsertAbsence(int employeeId, DateTime fromDate, DateTime toDate, string note, absenceReason reason)
        {
            DateTime fromDateWithTime = fromDate.Date.Add(FromTime.ToTimeSpan());

            DateTime toDateWithTime = toDate.Date.Add(ToTime.ToTimeSpan());

            return dbHelper.InsertAbsence(employeeId, fromDateWithTime, toDateWithTime, note, reason);
        }

        public void EditAbsence(List<Absence> absence)
        {
            dbHelper.EditAbsence(absence);
        }

        public void DeleteAbsence(int id)
        {
            dbHelper.DeleteAbsence(id);
        }
        
        public static List<Absence> GetAllAbsence(Employee employee)
        {
            DatabaseHelper databHelper = new ();

            return databHelper.GetAllAbsence(employee);
        }

        public void RevertToPreviousState()
        {
            FromDate = _oldFromDate;
            ToDate = _oldToDate;
            Note = _oldNote;
            AbsenceReason = _oldReason;
        }
        public Absence()
        {
        }
    }
}
