using System.Collections.ObjectModel;
using CheckInSystem.Database;
using CheckInSystem.Models;
using CheckInSystem.Platform;
using CheckInSystem.Views.UserControls;

namespace CheckInSystem.ViewModels.UserControls;

public class EmployeeTimeViewModel : ViewModelBase
{
    Absence absence;
    public List<Absence.AbsenceReason> absencesOptions { get; set; }
    public ObservableCollection<Absence> Absences { get; set; }
    public List<Absence> AbsencesToAddToDb { get; set; }
    public List<Absence> AbsencesToDelete { get; set; }

    public ObservableCollection<OnSiteTime> SiteTimes { get; set; }
    public List<OnSiteTime> SiteTimesToDelete { get; set; }
    public List<OnSiteTime> SiteTimesToAddToDb { get; set; }

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

        Absences = new();
        AbsencesToAddToDb = new();
        AbsencesToDelete = new();
        absencesOptions = new List<Absence.AbsenceReason>(Enum.GetValues(typeof(Absence.AbsenceReason)).Cast<Absence.AbsenceReason>());
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
        _platform.MainWindowViewModel.RequestView(typeof(AdminPanel));
    }

    public void SaveChanges()
    {
        UpdateSiteTimes();
        DeleteSiteTimes();
        AddSiteTimes();

        DeleteAbsences();
        AddAbsences();
        SelectedEmployee.GetUpdatedSiteTimes();
        _platform.MainWindowViewModel.RequestView(typeof(AdminPanel));
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
    public void AppendAbsenceToAddToDb(Absence absence)
    {
        Absences.Add(absence);
        AbsencesToAddToDb.Add(absence);

    }

    public void AppendAbsenceToDelete(Absence absence)
    {
        AbsencesToDelete.Add(absence);
        Absences.Remove(absence);
        AbsencesToAddToDb.Remove(absence); 
    }

    public void RevertAbsences()
    {
        foreach (var absence in Absences)
        {
            absence.RevertToPreviousState(); 
        }
    }

    private void AddAbsences()
    {
        foreach (var absence in AbsencesToAddToDb)
        {
            absence.InsertAbsence(absence._employeeId, absence._fromDate, absence._toDate, absence._note, absence._reason);
        }
        AbsencesToAddToDb.Clear();
    }

    private void DeleteAbsences()
    {
        foreach (var absence in AbsencesToDelete)
        {
            absence.DeleteAbsence(absence._id);
        }
        AbsencesToDelete.Clear();
    }
}