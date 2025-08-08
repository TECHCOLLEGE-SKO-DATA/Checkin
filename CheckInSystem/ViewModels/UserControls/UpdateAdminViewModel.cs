using CheckinLibrary.Database;
using CheckinLibrary.Models;
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
    public class UpdateAdminViewModel : ViewModelBase
    {
        public string txtUserName {  get; set; }
        public string txtPassword { get; set; }

        public string OldPassword { get; set; }
        public string OldUsername {  get; set; }

        AdminUser adminUser { get; set; } = new AdminUser();

        public ReactiveCommand<Unit, Unit> btn_UpdateAdmin { get; }
        public UpdateAdminViewModel(IPlatform platform) : base(platform)
        {
            btn_UpdateAdmin = ReactiveCommand.Create(() => UpdateAdmin());
        }

        public void UpdateAdmin()
        {
            adminUser.UpdateUser(txtUserName, txtPassword, OldUsername, OldPassword);

            _platform.MainWindowViewModel.SwitchToAdminPanel();
        }
    }
}
