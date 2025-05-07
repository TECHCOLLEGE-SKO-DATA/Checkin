using CheckinSystemAvalonia.Platform;
using CheckinSystemAvalonia.ViewModels.Windows;
using ReactiveUI;
using System.Reactive;
using CheckinLib.ViewModels.UserControls;

namespace CheckinSystemAvalonia.ViewModels.UserControls
{
    public class AdminLoginViewModel : ViewModelBase
    {
        LoginScreenViewModel loginScreenViewModel = new();

        private MainWindowViewModel _mainWindowViewModel;

        private string _userName;
        public string Username
        {
            get => _userName;
            set => this.RaiseAndSetIfChanged(ref _userName, value);
        }

        private string _passWord;
        public string PassWord
        {
            get => _passWord;
            set => this.RaiseAndSetIfChanged(ref _passWord, value);
        }

        // Reactive Command for login button click
        public ReactiveCommand<Unit, Unit> Btn_Login_Click { get; }

        public AdminLoginViewModel(IPlatform platform, MainWindowViewModel mainWindowViewModel) : base(platform)
        {
            _mainWindowViewModel = mainWindowViewModel;

            Btn_Login_Click = ReactiveCommand.Create(() => Login(Username, PassWord));
        }

        private void Login(string username, string passWord)
        {
            bool status = loginScreenViewModel.AdminLogin(username, passWord);
            
            if (status)
            {
                _mainWindowViewModel.SwitchToAdminPanel();
            }
        }
    }
}
