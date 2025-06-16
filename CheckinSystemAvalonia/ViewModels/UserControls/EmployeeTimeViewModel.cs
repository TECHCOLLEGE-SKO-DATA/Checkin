using Avalonia.Controls;
using CheckinLibrary.Models;
using CheckInSystemAvalonia.Platform;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace CheckInSystemAvalonia.ViewModels.UserControls
{
    public class EmployeeTimeViewModel : ViewModelBase
    {

        public List<AbsenceReason> AbsenceReasons { get; set; } = new();

        Absence absenc = new();
        public ObservableCollection<Absence> Absences { get; set; } = new();
        public List<Absence> AbsencesToAddToDb { get; set; } = new();
        public List<Absence> AbsencesToDelete { get; set; } = new();

        public ObservableCollection<OnSiteTime> SiteTimes { get; set; } = new();
        public List<OnSiteTime> SiteTimesToDelete { get; set; } = new();
        public List<OnSiteTime> SiteTimesToAddToDb { get; set; } = new();

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

                    this.RaisePropertyChanged(nameof(Absences));
                    this.RaisePropertyChanged(nameof(SiteTimes));
                }
            }
        }

        public ReactiveCommand<Unit, Unit> Btn_AddAbsence {  get; set; }

        public ReactiveCommand<Unit, Unit> Btn_LogOut { get; set; }

        public ReactiveCommand<Unit, Unit> Btn_Cancel { get; set; }

        public ReactiveCommand<Unit, Unit> Btn_Save { get; set; }

        public ReactiveCommand<Unit, Unit> Btn_AddSiteTime { get; set; }

        public EmployeeTimeViewModel(IPlatform platform) : base(platform)
        {
            platform.DataLoaded += (sender, args) =>
            {
                AbsenceReasons = platform.MainWindowViewModel.absenceReasons;
            };

            SiteTimesToDelete = new ();
            SiteTimesToAddToDb = new();

            Btn_LogOut = ReactiveCommand.Create(() => platform.MainWindowViewModel.SwitchToLoginView());

            Btn_Cancel = ReactiveCommand.Create(() => RevertSiteTimes()); 

            Btn_Save = ReactiveCommand.Create(() => SaveChanges());
            
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
            _platform.MainWindowViewModel.SwitchToAdminPanel();
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

            _platform.MainWindowViewModel.SwitchToAdminPanel();
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
                absence.InsertAbsence(absence.EmployeeId, absence.FromDate, absence.ToDate, absence.Note, absence.AbsenceReasonId);
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

        //added this since for some reason it would forget the Index of the absence for the reason of absence
        public void RefreshAbsences()
        {
            if (SelectedEmployee == null)
                return;

            Absences.Clear();

            var freshAbsences = Absence.GetAllAbsence(SelectedEmployee);
            foreach (var absence in freshAbsences)
            {
                Absences.Add(absence);
            }

            this.RaisePropertyChanged(nameof(Absences));
        }

    }
}
