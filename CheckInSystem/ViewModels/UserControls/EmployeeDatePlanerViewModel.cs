using Avalonia.Media;
using CheckinLibrary.Models;
using CheckInSystem.Platform;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace CheckInSystem.ViewModels.UserControls
{
    public class EmployeeDatePlanerViewModel : ViewModelBase
    {
        public ObservableCollection<PlannerEntry> Entries { get; } = new();

        public ObservableCollection<string> Hours { get; } = new ObservableCollection<string>(
            Enumerable.Range(0, 24).Select(h => $"{h:00}:00")
        );


        private const int HourHeight = 50;   // pixels per hour
        private const int DayWidth = 150;    // width of a column (adjust to taste)

        private void RefreshEntries()
        {
            Entries.Clear();

            // --- Absences ---
            foreach (var absence in Absences)
            {
                var start = absence.FromDate;
                var end = absence.ToDate;

                var entry = new PlannerEntry
                {
                    DayOffset = GetDayOffset(start.DayOfWeek),
                    StartY = start.Hour * HourHeight + start.Minute * (HourHeight / 60.0),
                    Height = (end - start).TotalHours * HourHeight,
                    Label = "Absent",
                    Color = Brushes.LightGray,
                    Opacity = 0.5
                };
                Entries.Add(entry);
            }

            // --- SiteTimes ---
            foreach (var siteTime in SiteTimes)
            {
                if (siteTime.ArrivalTime == null || siteTime.DepartureTime == null)
                    continue;

                var entry = new PlannerEntry
                {
                    DayOffset = GetDayOffset(siteTime.ArrivalTime.Value.DayOfWeek),
                    StartY = siteTime.ArrivalTime.Value.Hour * HourHeight +
                             siteTime.ArrivalTime.Value.Minute * (HourHeight / 60.0),
                    Height = ((siteTime.DepartureTime ?? DateTime.Now) - siteTime.ArrivalTime.Value).TotalHours * HourHeight,
                    Label = "Checked In",
                    Color = Brushes.LightGreen,
                    Opacity = 1
                };
                Entries.Add(entry);
            }
        }

        /// <summary>
        /// Maps DayOfWeek to X offset in canvas
        /// (Monday = first column, Friday = last)
        /// </summary>
        private double GetDayOffset(DayOfWeek day)
        {
            // skip Sunday/Saturday
            return day switch
            {
                DayOfWeek.Monday => 1 * DayWidth,
                DayOfWeek.Tuesday => 2 * DayWidth,
                DayOfWeek.Wednesday => 3 * DayWidth,
                DayOfWeek.Thursday => 4 * DayWidth,
                DayOfWeek.Friday => 5 * DayWidth,
                _ => 0
            };
        }

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
                if (_selectedEmployee != value)
                {
                    SetProperty(ref _selectedEmployee, value);

                    Absences.Clear();
                    foreach (var absence in Absence.GetAllAbsence(value))
                        Absences.Add(absence);

                    SiteTimes.Clear();
                    foreach (var siteTime in OnSiteTime.GetOnsiteTimesForEmployee(value))
                        SiteTimes.Add(siteTime);

                    // after data reload, rebuild entries for the planner
                    RefreshEntries();

                    this.RaisePropertyChanged(nameof(Absences));
                    this.RaisePropertyChanged(nameof(SiteTimes));
                }
            }
        }

        public ReactiveCommand<Unit, Unit> Btn_AddAbsence { get; }

        public ReactiveCommand<Unit, Unit> Btn_AddSiteTime { get; }

        public ReactiveCommand<Unit, Unit> Btn_Back { get; }

        public ReactiveCommand<Unit, Unit> Btn_Logout { get; }

        public ReactiveCommand<Unit, Unit> Btn_Save {  get; }

        public EmployeeDatePlanerViewModel(IPlatform platform) : base(platform) 
        {
            platform.DataLoaded += (sender, args) =>
            {
                AbsenceReasons = platform.MainWindowViewModel.absenceReasons;
            };

            Btn_Back = ReactiveCommand.Create(() => _platform.MainWindowViewModel.SwitchToAdminPanel());
            Btn_Logout = ReactiveCommand.Create(() => _platform.MainWindowViewModel.SwitchToLoginView());
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

            _platform.MainWindowViewModel.absencBackGroundService.AbsenceTask();

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

    public class PlannerEntry
    {
        public double DayOffset { get; set; }   // X position (per weekday column)
        public double StartY { get; set; }      // Y position (based on time of day)
        public double Height { get; set; }      // Duration in pixels
        public string Label { get; set; }       // Display text
        public IBrush Color { get; set; }       // Background
        public double Opacity { get; set; }     // 1.0 for normal, 0.5 for absence
    }
}
