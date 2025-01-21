using System.Collections.ObjectModel;
using CheckInSystem.Models;

namespace CheckInSystem.ViewModels.UserControls;

public class EmployeeTimeViewModel
{
    public ObservableCollection<OnSiteTime> SiteTimes { get; set; }
    public List<OnSiteTime> SiteTimesToDelete { get; set; }
    public List<OnSiteTime> SiteTimesToAddToDb { get; set; }
    public Employee SelectedEmployee { get; set; }

    public EmployeeTimeViewModel(Employee employee)
    {
        SelectedEmployee = employee;
        SiteTimesToDelete = new();
        SiteTimesToAddToDb = new();
        
        SiteTimes = new(OnSiteTime.GetOnsiteTimesForEmployee(employee));
    }

    public void AppendSiteTimesToDelete(OnSiteTime siteTime)
    {
        SiteTimesToDelete.Add(siteTime);
        SiteTimes.Remove(siteTime);
        SiteTimesToAddToDb.Remove(siteTime);
    }

    public void AppendSiteTimesToAddToDb(OnSiteTime siteTime)
    {
        SiteTimes.Add(siteTime);
        SiteTimesToAddToDb.Add(siteTime);
    }

    public void RevertSiteTimes()
    {
        foreach (var siteTime in SiteTimes)
        {
            siteTime.RevertTopreviousTime();
        }
    }

    public void SaveChanges()
    {
        UpdateSiteTimes();
        DeleteSiteTimes();
        AddSiteTimes();
        SelectedEmployee.GetUpdatedSiteTimes();
    }

    private void UpdateSiteTimes()
    {
        List<OnSiteTime> changedSiteTimes = new List<OnSiteTime>();
        foreach (var siteTime in SiteTimes)
        {
            if (siteTime.IsChanged())
            {
                changedSiteTimes.Add(siteTime);
            }
        }
        if (SiteTimes.Count > 0)
        {
            OnSiteTime.UpdateMutipleSiteTimes(changedSiteTimes);
        }
    }

    private void DeleteSiteTimes()
    {
        foreach (var siteTime in SiteTimesToDelete)
        {
            siteTime.DeleteFromDb();
        }
        SiteTimesToDelete.Clear();
    }

    private void AddSiteTimes()
    {
        foreach (var siteTime in SiteTimesToAddToDb)
        {
            if (siteTime.ArrivalTime != null)
            {
                OnSiteTime.AddTimeToDb(siteTime.EmployeeID, siteTime.ArrivalTime ?? DateTime.Now, siteTime.DepartureTime);
            }
        }
        SiteTimesToAddToDb.Clear();
    }
}