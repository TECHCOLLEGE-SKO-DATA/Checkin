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

        private DatabaseHelper dbHelper;

        // Original (backup) state
        private DateTime _oldFromDate;
        private DateTime _oldToDate;
        private string _oldNote;
        private AbsenceReason _oldReason;

        // Current state properties
        public int _id { get; private set; }
        public int _employeeId { get; private set; }
        public DateTime _fromDate { get; private set; }
        public DateTime _toDate { get; private set; }
        public string _note { get; private set; }
        public AbsenceReason _reason { get; set; }

        public Absence(int id, int employeeId, DateTime fromDate, DateTime toDate, string note, AbsenceReason reason)
        {
            _id = id;
            _employeeId = employeeId;

            // Initialize both the current and backup states
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
            // Update the current state
            _fromDate = fromDate;
            _toDate = toDate;
            _note = note;
            _reason = reason;

            // Persist changes in the database
            dbHelper.EditAbsence(fromDate, toDate, note, reason);
        }

        public void DeleteAbsence(int id)
        {
            dbHelper.DeleteAbsence(id);
        }

        public void GetAllAbsence(int employeeId)
        {
            dbHelper.GetAllAbsence(employeeId);
        }

        // New method: Revert to previous state
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
