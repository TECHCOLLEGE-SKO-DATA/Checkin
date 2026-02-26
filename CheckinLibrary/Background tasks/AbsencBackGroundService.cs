using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CheckinLibrary.Database;
using CheckinLibrary.Models;

namespace CheckinLibrary.Background_tasks;

public class AbsencBackGroundService
{
    private TimeSpan waitTime = TimeSpan.FromMinutes(2);

    public List<Employee> employees = new List<Employee>();

    public void AbsenceTask()
    {
        List<Task> offsiteTasks = new();

        var employeesCopy = employees.ToList(); 

        foreach (var employee in employeesCopy)
        {
            var activeAbsences = Absence.GetAllAbsence(employee)
                .Where(a => a.FromDate.Date == DateTime.Now.Date)
                .ToList();

            if (activeAbsences.Any())
            {
                offsiteTasks.Add(Task.Run(async () =>
                {
                    lock (employees) 
                    {
                        employees.Remove(employee);
                    }

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
        activeAbsences = activeAbsences
            .OrderBy(a => a.FromDate)
            .ToList();

        foreach (var activeAbsence in activeAbsences)
        {
            bool hasStarted = false;

            while (true)
            {
                var currentAbsences = Absence.GetAllAbsence(employee)
                                             .Where(a => a.ID == activeAbsence.ID)
                                             .ToList();

                if (!currentAbsences.Any())
                {
                    // Absence was deleted before or during
                    employee.IsOffSite = false;
                    break;
                }

                var currentAbsence = currentAbsences[0];

                // Update dates in case they've changed
                activeAbsence.FromDate = currentAbsence.FromDate;
                activeAbsence.ToDate = currentAbsence.ToDate;

                if (!hasStarted && DateTime.Now >= activeAbsence.FromDate)
                {
                    hasStarted = true;
                    employee.IsOffSite = true;
                }

                if (hasStarted && DateTime.Now >= activeAbsence.ToDate)
                {
                    employee.IsOffSite = false;
                    break;
                }

                await Task.Delay(waitTime);
            }

            break;
        }

        return employee;
    }

    public void AddEmployeesToAbsenceCheck(Employee employee)
    {
        employees.Add(employee);
    }
}
