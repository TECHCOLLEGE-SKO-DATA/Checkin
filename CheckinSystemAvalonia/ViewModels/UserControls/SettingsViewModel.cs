using Avalonia.Media;
using CheckinLibrary.Models;
using CheckinLibrary.Settings;
using CheckInSystemAvalonia.Platform;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Reactive;
using static Dapper.SqlMapper;

namespace CheckInSystemAvalonia.ViewModels.UserControls
{
    public class SettingsViewModel : ViewModelBase
    {
        public ObservableCollection<AbsenceReason> AbsenceReasons { get; set; } = new();

        //Buttons
        public ReactiveCommand<Unit, Unit> Btn_AddValidAbsence { get; }

        public ReactiveCommand<Unit, Unit> Btn_BackToAdminPanel { get; }

        public ReactiveCommand<Unit, Unit> Btn_Logout { get; }

        public ReactiveCommand<AbsenceReason, Unit> Btn_DeleteReason { get; }

        public ReactiveCommand<Unit, Unit> Btn_Save { get; }

        public SettingsViewModel(IPlatform platform) : base(platform)
        {
            platform.DataLoaded += (sender, args) =>
            {
                AbsenceReasons.Clear();
                foreach (var reason in platform.MainWindowViewModel.absenceReasons)
                    AbsenceReasons.Add(reason);
            };

            Btn_DeleteReason = ReactiveCommand.Create<AbsenceReason>(DeleteAbsenceReason);

            Btn_AddValidAbsence = ReactiveCommand.Create(() => AddValidAbsence());

            Btn_BackToAdminPanel = ReactiveCommand.Create(() => platform.MainWindowViewModel.SwitchToAdminPanel());

            Btn_Logout = ReactiveCommand.Create(() => platform.MainWindowViewModel.SwitchToLoginView());

            Btn_Save = ReactiveCommand.Create(() => SaveChanges());
        }

        public void DeleteAbsenceReason(AbsenceReason absenceReason)
        {
            if (absenceReason != null && AbsenceReasons.Contains(absenceReason))
                AbsenceReasons.Remove(absenceReason);

        }

        public void AddValidAbsence()
        {
            int newId = AbsenceReasons.Any() ? AbsenceReasons.Max(r => r.Id) + 1 : 1;

            System.Drawing.Color hexColor = ColorTranslator.FromHtml("#3498db");

            AbsenceReason absence = new AbsenceReason(newId, "New Reason", hexColor);

            AbsenceReasons.Add(absence);
        }

        private void SaveChanges()
        {
            foreach (var item in AbsenceReasons)
            {
                var existing = _platform.MainWindowViewModel.absenceReasons.FirstOrDefault(x => x.Id == item.Id);
                if (existing != null)
                {
                    existing.Reason = item.Reason;
                    existing.HexColor = item.HexColor;
                }
                else
                {
                    _platform.MainWindowViewModel.absenceReasons.Add(new AbsenceReason(item.Id, item.Reason, item.HexColor));
                }
            }

            SettingsControl settings = new();
            settings.SetAbsenceReasons(AbsenceReasons.ToList());

            _platform.MainWindowViewModel.SwitchToAdminPanel();
        }
    }
}
