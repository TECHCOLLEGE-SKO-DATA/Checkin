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
            _oldFromDate = FromDate = absence.FromDate;
            _oldToDate = ToDate = absence.ToDate;
            _oldNote = Note = absence.Note;
            _oldReason = AbsenceReason = absence.AbsenceReason;
        }
        public Absence(int id, int employeeId, DateTime fromDate, DateTime toDate, string note, absenceReason reason)
        {
            ID = id;
            EmployeeId = employeeId;

            FromDate = _oldFromDate = fromDate;
            ToDate = _oldToDate = toDate;
            Note = _oldNote = note;
            AbsenceReason = _oldReason = reason;
        }

        public Absence InsertAbsence(int employeeId, DateTime fromDate, DateTime toDate, string note, absenceReason reason)
        {
            return dbHelper.InsertAbsence(employeeId, fromDate, toDate, note, reason);
        }

        public void EditAbsence(DateTime fromDate, DateTime toDate, string note, absenceReason reason)
        {
            FromDate = fromDate;
            ToDate = toDate;
            Note = note;
            AbsenceReason = reason;

            dbHelper.EditAbsence(fromDate, toDate, note, reason);
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
