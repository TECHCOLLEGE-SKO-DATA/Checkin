using CheckinSystemAvalonia.Platform;
using CheckinSystemAvalonia.ViewModels.Windows;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace CheckinSystemAvalonia.ViewModels.UserControls
{
    public class AdminGroupViewModel : ViewModelBase
    {
        public ReactiveCommand<Unit, Unit> Btn_AdminPanel { get; }
        public AdminGroupViewModel(IPlatform platform) : base(platform)
        {
            _platform = platform;
            Btn_AdminPanel = ReactiveCommand.Create(() => _platform.MainWindowViewModel.SwitchToAdminPanel());
        }
    }
}
