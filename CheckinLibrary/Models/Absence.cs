using CheckinLibrary.Background_tasks;
using CheckinLibrary.Database;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CheckinLibrary.Models
{
    public class Absence
    {
        private DatabaseHelper dbHelper = new();

        public TimeOnly FromTime { get; set; }
        public TimeOnly ToTime { get; set; }

        public int ID { get; set; }
        public int EmployeeId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int AbsenceReasonId { get; set; }

        public AbsenceReason AbsenceReason => AbsenceReason.GetById(AbsenceReasonId);

        public string? Note { get; set; }

        public DateTimeOffset FromDateOffset
        {
            get => new DateTimeOffset(FromDate);
            set => FromDate = value.DateTime;
        }

        public DateTimeOffset ToDateOffset
        {
            get => new DateTimeOffset(ToDate);
            set => ToDate = value.DateTime;
        }

        public Absence() { }

        public Absence(Absence absence)
        {
            ID = absence.ID;
            EmployeeId = absence.EmployeeId;
            FromDate = absence.FromDate;
            ToDate = absence.ToDate;
            Note = absence.Note;
            AbsenceReasonId = absence.AbsenceReasonId;
            FromTime = absence.FromTime;
            ToTime = absence.ToTime;
        }

        public Absence(int id, int employeeId, DateTime fromDate, DateTime toDate, string note, int reasonId)
        {
            ID = id;
            EmployeeId = employeeId;
            FromDate = fromDate;
            ToDate = toDate;
            Note = note;
            AbsenceReasonId = reasonId;
            FromTime = TimeOnly.FromDateTime(fromDate);
            ToTime = TimeOnly.FromDateTime(toDate);
        }

        public Absence InsertAbsence(int employeeId, DateTime fromDate, DateTime toDate, string note, int reasonId)
        {
            DateTime fromDateWithTime = fromDate.Date.Add(FromTime.ToTimeSpan());
            DateTime toDateWithTime = toDate.Date.Add(ToTime.ToTimeSpan());

            return dbHelper.InsertAbsence(employeeId, fromDateWithTime, toDateWithTime, note, reasonId);
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
            DatabaseHelper databHelper = new();
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
    }

    public class AbsenceReason
    {
        public int Id { get; private set; }
        public string Reason { get; set; }
        public Color HexColor { get; set; }

        public AbsenceReason(int id, string reason, Color hexColor)
        {
            Id = id;
            Reason = reason;
            HexColor = hexColor;
        }

        public static List<AbsenceReason> Reasons = new();

        public static AbsenceReason GetById(int id) => Reasons.FirstOrDefault(r => r.Id == id);
    }
}

