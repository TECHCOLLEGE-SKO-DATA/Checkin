using CheckInSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class BackgroundTimeService
{
    Absence absence = new();

    private TimeSpan waitTime = TimeSpan.FromMinutes(10);

    public List<Employee> employees = new List<Employee>();

    public void AbsenceTask()
    {
        List<Task> offsiteTasks = new(); // Stores async tasks for each employee

        bool some;

        foreach (var employee in employees)
        {
            var activeAbsences = Absence.GetAllAbsence(employee)
                .Where(a => a.ToDate > DateTime.Now)
                .ToList();

            if (activeAbsences.Any()) // Checks if absences exist
            {
                // Run OffsiteTimer asynchronously and store the task
                offsiteTasks.Add(Task.Run(async () =>
                {
                    employees.Remove(employee);

                    var (emp,some) = await OffsiteTimer(employee, activeAbsences);

                    employees.Add(emp);
                }));
            }
        }
    }

    public async Task<(Employee,bool)> OffsiteTimer(Employee employee, List<Absence> activeAbsences)
    {
        

        foreach (var activeAbsence in activeAbsences)
        {
            TimeSpan delayTime = activeAbsence.FromDate - DateTime.Now;

            if (delayTime > TimeSpan.Zero) // Ensure the delay is only applied if it's in the future
            {
                await Task.Delay(delayTime);
            }



            employee.IsOffSite = true;

            while (activeAbsence.ToDate > DateTime.Now)
            {
                var CurrentAbsences = Absence.GetAllAbsence(employee)
                            .Where(a => a.ID == activeAbsence.ID);

                foreach (var absence in CurrentAbsences)
                    activeAbsence.FromDate = absence.FromDate;

                if (!CurrentAbsences.Any(a => a.ID == activeAbsence.ID))
                    break;

                var timeUntilStart = activeAbsence.FromDate - DateTime.Now;

                //it uses whatever time is shortest for the constent checking if the absence still exist
                await Task.Delay(timeUntilStart < waitTime ? timeUntilStart : waitTime);
            }

            employee.IsOffSite = false;
            break;
        }
        return (employee, false);
    }

    public void AddEmployeesToAbsenceCheck(Employee employee)
    {
        employees.Add(employee);
    }
}

