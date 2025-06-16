using Avalonia.Media;
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
    public class SettingsViewModel : ViewModelBase
    {
        public List<AbsenceReason> AbsenceReasons { get; set; } = new();
        
        //Buttons

        public ReactiveCommand<Unit, Unit> Btn_DeleteReason { get; }

        public SettingsViewModel(IPlatform platform) : base(platform)
        {
            AbsenceReasons = platform.MainWindowViewModel.absenceReasons;
        }
        public void DeleteAbsenceReason(AbsenceReason absenceReason)
        {

        }
    }
}
