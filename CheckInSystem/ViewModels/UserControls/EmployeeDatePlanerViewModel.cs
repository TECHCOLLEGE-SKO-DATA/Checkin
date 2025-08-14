using CheckInSystem.Platform;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace CheckInSystem.ViewModels.UserControls
{
    public class EmployeeDatePlanerViewModel : ViewModelBase
    {
        public ReactiveCommand<Unit, Unit> Btn_Back { get; }

        public ReactiveCommand<Unit, Unit> Btn_Logout { get; }
        public EmployeeDatePlanerViewModel(IPlatform platform) : base(platform) 
        { 
            Btn_Back = ReactiveCommand.Create(() => _platform.MainWindowViewModel.SwitchToAdminPanel());
            Btn_Logout = ReactiveCommand.Create(() => _platform.MainWindowViewModel.SwitchToLoginView());
        }  
    }
}
