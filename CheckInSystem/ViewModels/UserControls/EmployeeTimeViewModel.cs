﻿using System.Collections.ObjectModel;
using CheckInSystem.Models;
using CheckInSystem.Platform;
using CheckInSystem.Views.UserControls;

namespace CheckInSystem.ViewModels.UserControls;

public class EmployeeTimeViewModel : ViewModelBase
{
    Absence absenc = new();
    public ObservableCollection<Absence> Absences { get; set; } = new();
    public List<Absence> AbsencesToAddToDb { get; set; } = new();
    public List<Absence> AbsencesToDelete { get; set; } = new();

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
            if (_selectedEmployee != value) // Ensure we only update if different
            {
                SetProperty(ref _selectedEmployee, value);

                Absences.Clear();
                foreach (var absence in Absence.GetAllAbsence(value))
                {
                    Absences.Add(absence);
                }

                SiteTimes.Clear();
                foreach (var siteTime in OnSiteTime.GetOnsiteTimesForEmployee(value))
                {
                    SiteTimes.Add(siteTime);
                }

                OnPropertyChanged(nameof(Absences));
                OnPropertyChanged(nameof(SiteTimes));
            }
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

        UpdateAbsenceTimes();
        DeleteAbsences();
        AddAbsences();

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











    private void AddAbsences()

    {

        foreach (var absence in AbsencesToAddToDb)

        {

            absence.InsertAbsence(absence.EmployeeId, absence.FromDate, absence.ToDate, absence.Note, absence.AbsenceReason);

        }

        AbsencesToAddToDb.Clear();

    }



    private void DeleteAbsences()
    {

        foreach (var absence in AbsencesToDelete)
        {

            absence.DeleteAbsence(absence.ID);

        }

        AbsencesToDelete.Clear();

    }

    private void UpdateAbsenceTimes()

    {

        List<Absence> changedAbsence = new List<Absence>();

        foreach (var absence in Absences)

        {

            absence.FromDate = absence.FromDate.Date.Add(absence.FromTime.ToTimeSpan());



            absence.ToDate = absence.ToDate.Date.Add(absence.ToTime.ToTimeSpan());



            changedAbsence.Add(absence);

        }

        if (Absences.Count > 0)

        {

            absenc.EditAbsence(changedAbsence);

        }
    }
}