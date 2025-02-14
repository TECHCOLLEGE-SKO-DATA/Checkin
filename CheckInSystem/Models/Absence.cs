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

        DatabaseHelper dbHelper;

        public int _id { get; private set; }
        public int _employeeId { get; private set; }
        public DateTime _fromDate { get; private set; }
        public DateTime _toDate { get; private set; }
        public string _note { get; private set; }
        public AbsenceReason _reason { get; private set; }

        public Absence(int id, int employeeId, DateTime fromDate, DateTime toDate, string note, AbsenceReason reason)
        {
            _id = id;
            _employeeId = employeeId;
            _fromDate = fromDate;
            _toDate = toDate;
            _note = note;
            _reason = reason;
        }

        public Absence InsertAbsence(int _employeeId ,DateTime _fromDate,DateTime _toDate, string _note , AbsenceReason _reason)
        {
            return dbHelper.InsertAbsence(_employeeId, _fromDate, _toDate, _note, _reason);
        }
        public void EditAbsence(DateTime _fromDate, DateTime _toDate, string _note, AbsenceReason _reason)
        {
            dbHelper.EditAbsence(_fromDate, _toDate, _note, _reason);
        }
        public void DeleteAbsence(int _id) 
        {
            dbHelper.DeleteAbsence(_id);
        }
        public void GetAllAbsence(int _employeeId)
        {
            dbHelper.GetAllAbsence(_employeeId);
        }

        public Absence()
        {

        }
    }
}
