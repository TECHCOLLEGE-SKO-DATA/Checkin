using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using CheckinLibrary.Database;
using CheckinLibrary.Models;
using CheckInSystem.Controls;
using CheckInSystem.Platform;
using CheckInSystem.ViewModels.Windows;
using DynamicData;
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
    public class AdminsViewModel : ViewModelBase
    {
        AdminUser adminUser = new();

        public ObservableCollection<AdminItemViewModel> AdminList { get; set; } = new();

        public string txtUsername {  get; set; }

        public string txtPassword { get; set; }

        //buttons
        public ReactiveCommand<Unit, Unit> Btn_AddAdmin { get; }

        public ReactiveCommand<Unit, Unit> Btn_Back { get; }

        public ReactiveCommand<Unit, Unit> Btn_Logout { get; }

        public ReactiveCommand<AdminItemViewModel, Unit> UpdateAdminCommand { get; }
        
        public ReactiveCommand<AdminItemViewModel, Unit> DeleteAdminCommand {  get; }

        public AdminsViewModel(IPlatform platform) : base(platform)
        {
            if (!Design.IsDesignMode)
            {
                var users = adminUser.GetAdminUsers();
                foreach (var u in users)
                    AdminList.Add(new AdminItemViewModel(u));
            }

            UpdateAdminCommand = ReactiveCommand.Create<AdminItemViewModel>(UpdateAdmin);

            DeleteAdminCommand = ReactiveCommand.CreateFromTask<AdminItemViewModel>(DeleteAdminAsync);

            Btn_AddAdmin = ReactiveCommand.Create(() => 
            {
                adminUser.CreateUser(txtUsername, txtPassword);
                txtUsername = "";
                txtPassword = "";
            });

            Btn_Back = ReactiveCommand.Create(() => {platform.MainWindowViewModel.SwitchToAdminPanel();});
            Btn_Logout = ReactiveCommand.Create(() => {platform.MainWindowViewModel.SwitchToLoginView();});
        }
        private void UpdateAdmin(AdminItemViewModel adminItem)
        {
            _platform.Database.UpdateUser(adminItem.User.Username, adminItem.NewPassword, adminItem.User.ID);
            adminItem.NewPassword = "";
        }

        private async Task DeleteAdminAsync(AdminItemViewModel adminItem)
        {
            if (adminItem == null) return;

            var user = adminItem.User;

            var result = await MessageBox.Show(
                _platform.MainWindow,
                $"Er du sikker på at du vil slette {user.Username}?",
                "Sletning",
                MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                adminUser.Delete(adminItem.User.ID);
                AdminList.Remove(adminItem);
            }
        }
    }

    public class AdminItemViewModel : ReactiveObject
    {
        public AdminUser User { get; }

        private string _newPassword;
        public string NewPassword
        {
            get => _newPassword;
            set => this.RaiseAndSetIfChanged(ref _newPassword, value);
        }

        public AdminItemViewModel(AdminUser user)
        {
            User = user;
        }
    }
}
