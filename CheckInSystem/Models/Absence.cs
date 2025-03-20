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

        private TimeSpan waitTime = TimeSpan.FromMinutes(5);

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

        public async Task OffsiteTimer(Employee employee)
        {
            while (true == true)
            {
                var activeAbsences = Absence.GetAllAbsence(employee)
                        .Where(a => a.ToDate > DateTime.Now).ToList();

                if (activeAbsences == null)
                    return;

                foreach (var activeAbsence in activeAbsences)
                {
                    // Wait until FromDate while checking every 5 minutes if the absence still exists
                    while (activeAbsence.FromDate > DateTime.Now)
                    {
                        var CurrentAbsences = Absence.GetAllAbsence(employee)
                            .Where(a => a.ID == activeAbsence.ID);

                        foreach (var absence in CurrentAbsences)
                            activeAbsence.FromDate = absence.FromDate;

                        if (!CurrentAbsences.Any(a => a.ID == activeAbsence.ID))
                            return;

                        var timeUntilStart = activeAbsence.FromDate - DateTime.Now;

                        //it uses whatever time is shortest for the constent checking if the absence still exist
                        await Task.Delay(timeUntilStart < waitTime ? timeUntilStart : waitTime);
                    }

                    employee.IsOffSite = true;

                    // Wait until ToDate while checking every 5 minutes if the absence still exists
                    while (activeAbsence.ToDate > DateTime.Now)
                    {
                        var CurrentAbsences = Absence.GetAllAbsence(employee)
                            .Where(a => a.ID == activeAbsence.ID);

                        foreach (var absence in CurrentAbsences)
                            activeAbsence.ToDate = absence.ToDate;

                        if (!CurrentAbsences.Any(a => a.ID == activeAbsence.ID))
                        {
                            employee.IsOffSite = false;

                            return;
                        }

                        var timeUntilEnd = activeAbsence.ToDate - DateTime.Now;

                        //it uses whatever time is shortest for the constent checking if the absence still exist
                        await Task.Delay(timeUntilEnd < waitTime ? timeUntilEnd : waitTime);

                    }

                    employee.IsOffSite = false;

                }
                await Task.Delay(waitTime);
            }
        }
        public Absence()
        {
        }
    }
}
