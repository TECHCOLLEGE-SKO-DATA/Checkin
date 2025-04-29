using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CheckinLib.Models;

namespace CheckinLib.Background_tasks;

public class AbsencBackGroundService
{
    Absence absence = new();

    private TimeSpan waitTime = TimeSpan.FromMinutes(10);

    public List<Employee> employees = new List<Employee>();

    public void AbsenceTask()
    {
        List<Task> offsiteTasks = new();
        var employeesCopy = employees.ToList(); // Create a copy

        foreach (var employee in employeesCopy)
        {
            var activeAbsences = Absence.GetAllAbsence(employee)
                .Where(a => a.ToDate > DateTime.Now)
                .ToList();

            if (activeAbsences.Any())
            {
                offsiteTasks.Add(Task.Run(async () =>
                {
                    lock (employees) // Ensure thread safety
                    {
                        employees.Remove(employee);
                    }// Lock is released here for other tasks to do what they need

                    var emp = await OffsiteTimer(employee, activeAbsences);

                    lock (employees)
                    {
                        employees.Add(emp);
                    }
                }));
            }
        }
    }


    public async Task<Employee> OffsiteTimer(Employee employee, List<Absence> activeAbsences)
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
        return employee;
    }

    public void AddEmployeesToAbsenceCheck(Employee employee)
    {
        employees.Add(employee);
    }
}
