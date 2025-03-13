using System.Collections.ObjectModel;
using CheckInSystem.Models;
using CheckInSystem.Platform;

namespace CheckInSystem.ViewModels.UserControls;

public class EmployeeTimeViewModel : ViewModelBase
{
    public ObservableCollection<OnSiteTime> SiteTimes { get; set; } = new();
    public List<OnSiteTime> SiteTimesToDelete { get; set; } = new();
    public List<OnSiteTime> SiteTimesToAddToDb { get; set; } = new();
    //public Employee SelectedEmployee { get; set; }

    Employee _selectedEmployee = new();
    public Employee SelectedEmployee
    {
        get => _selectedEmployee;
        set
        {
            SiteTimes = new(OnSiteTime.GetOnsiteTimesForEmployee(value));
            SetProperty(ref _selectedEmployee, value, nameof(SiteTimes));
        }
    }

    public EmployeeTimeViewModel(IPlatform platform) : base(platform)
    {
        //SelectedEmployee = employee;
        SiteTimesToDelete = new();
        SiteTimesToAddToDb = new();
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
        _platform.MainWindowViewModel.RequestView(typeof(AdminPanelViewModel));
    }

    public void SaveChanges()
    {
        UpdateSiteTimes();
        DeleteSiteTimes();
        AddSiteTimes();
        SelectedEmployee.GetUpdatedSiteTimes();
        _platform.MainWindowViewModel.RequestView(typeof(AdminPanelViewModel));
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