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

            Btn_GroupView = ReactiveCommand.Create(() => SwitchToGroupView());

            Btn_SettingsView = ReactiveCommand.Create(() => SwitchToSettingsView());

            Btn_LoginView = ReactiveCommand.Create(() => SwitchToLoginView());
        }

        public void SwitchToGroupView()
        {
            _mainWindowViewModel.CurrentViewModel = new AdminGroupViewModel();
        }

        public void SwitchToSettingsView()
        {
            _mainWindowViewModel.CurrentViewModel = new SettingsViewModel();
        }

        public void SwitchToLoginView()
        {
            _mainWindowViewModel.CurrentViewModel = new AdminLoginViewModel(_mainWindowViewModel);
        }
    }
}
