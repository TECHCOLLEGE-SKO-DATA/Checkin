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
        public enum AbsenceReason
        {
            Sick,
            School,
            Vacation
        }

        private DatabaseHelper dbHelper = new();

        
        private DateTime _oldFromDate;
        private DateTime _oldToDate;
        private string _oldNote;
        private AbsenceReason _oldReason;

        
        public int _id { get; private set; }
        public int _employeeId { get; private set; }
        public DateTime _fromDate { get; set; }
        public DateTime _toDate { get; set; }
        public string _note { get; set; }
        public AbsenceReason _reason { get; set; }

        public Absence(Absence absence)
        {
            _id = absence._id;
            _employeeId = absence._employeeId;

            _fromDate = _oldFromDate = absence._fromDate;
            _toDate = _oldToDate = absence._toDate;
            _note = _oldNote = absence._note;
            _reason = _oldReason = absence._reason;
        }
        public Absence(int id, int employeeId, DateTime fromDate, DateTime toDate, string note, AbsenceReason reason)
        {
            _id = id;
            _employeeId = employeeId;

            _fromDate = _oldFromDate = fromDate;
            _toDate = _oldToDate = toDate;
            _note = _oldNote = note;
            _reason = _oldReason = reason;
        }

        public Absence InsertAbsence(int employeeId, DateTime fromDate, DateTime toDate, string note, AbsenceReason reason)
        {
            return dbHelper.InsertAbsence(employeeId, fromDate, toDate, note, reason);
        }

        public void EditAbsence(DateTime fromDate, DateTime toDate, string note, AbsenceReason reason)
        {
            _fromDate = fromDate;
            _toDate = toDate;
            _note = note;
            _reason = reason;

            dbHelper.EditAbsence(fromDate, toDate, note, reason);
        }

        public void DeleteAbsence(int id)
        {
            dbHelper.DeleteAbsence(id);
        }

        public static List<Absence> GetAllAbsence(Employee employee)
        {
            DatabaseHelper databHelper = new ();

            return databHelper.GetAllAbsence(employee.ID);
        }

        public void RevertToPreviousState()
        {
            _fromDate = _oldFromDate;
            _toDate = _oldToDate;
            _note = _oldNote;
            _reason = _oldReason;
        }

        public Absence()
        {
        }
    }
}
