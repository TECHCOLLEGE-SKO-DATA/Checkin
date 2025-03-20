using CheckInSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CheckInSystem.Models;
using CheckInSystem.Database;

namespace CheckInSystem.Background_tasks
{
    public class AbsenceTimer
    {
        IDatabaseHelper databaseHelper;
        public List<Employee> employees = new();
        Absence absence = new();

        private TimeSpan waitTime = TimeSpan.FromMinutes(5);
        
        public async Task OffsiteEmployeeChecker()
        {
            employees = databaseHelper.GetAllEmployees();

            while (true == true)
            {
                foreach (var employee in employees)
                {
                    OffsiteTimer(employee);

                    employees.Remove(employee);
                }

                await Task.Delay(waitTime);
            }
        }

        public async Task OffsiteTimer(Employee employee)
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

            employees.Add(employee);
        }
    }
}
