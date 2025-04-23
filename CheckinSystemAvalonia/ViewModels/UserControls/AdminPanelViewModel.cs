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
    public class AdminPanelViewModel : ViewModelBase
    {

        public ReactiveCommand<Unit, Unit> Btn_GroupView { get; }

        public ReactiveCommand<Unit, Unit> Btn_SettingsView { get; }

        public ReactiveCommand<Unit, Unit> Btn_LoginView {  get; }

        private MainWindowViewModel _mainWindowViewModel;

        public AdminPanelViewModel(MainWindowViewModel mainWindowViewModel) 
        {
            _mainWindowViewModel = mainWindowViewModel;

            Btn_GroupView = ReactiveCommand.Create(() => _mainWindowViewModel.SwitchToGroupView());

            Btn_SettingsView = ReactiveCommand.Create(() => _mainWindowViewModel.SwitchToSettingsView());

            Btn_LoginView = ReactiveCommand.Create(() => _mainWindowViewModel.SwitchToLoginView());
        }
    }
}
