using CheckinLibrary.Database;
using CheckinLibrary.Models;
using CheckInSystemAvalonia.Platform;
using CheckInSystemAvalonia.ViewModels.Windows;
using ReactiveUI;
using System.Reactive;

namespace CheckInSystemAvalonia.ViewModels.UserControls
{
    public class AdminLoginViewModel : ViewModelBase
    {
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

        public AdminLoginViewModel(IPlatform platform) : base(platform)
        {
            _platform = platform;

            MainWindowViewModel main = _platform.MainWindowViewModel;

            Btn_Login_Click = ReactiveCommand.Create(() => Login(Username, PassWord));
        }

        private void Login(string username, string passWord)
        {
            DatabaseHelper databaseHelper = new();
            AdminUser? adminUser = databaseHelper.Login(Username, PassWord);
            if (adminUser == null)
            {
                //MessageBox.Show("Forkert brugernavn eller kodeord, prøv igen.", "Login fejl", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                _platform.MainWindowViewModel.SwitchToAdminPanel();
            }
        }
    }
}
