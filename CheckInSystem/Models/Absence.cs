using CheckInSystem.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CheckInSystem.Models
{
    public class Absence
    {
        public enum absenceReason
        {
            Syg,            
            Skole,
            Ferie,
            SøgeDag,
            VirksomhedSamtale,
            Læge,
            Miscellaneous
        }

        private DatabaseHelper dbHelper = new();

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
        public void SetIsOffSite(Employee employee)
        {
            List<Absence> absences = GetAllAbsence(employee);

            foreach (var absence in absences)
            {
                if (absence.FromDate <= DateTime.Now && absence.ToDate > DateTime.Now)
                {
                    employee.IsOffSite = true;
                    return; 
                }
            }
        }
        public Absence()
        {
        }
    }
}
