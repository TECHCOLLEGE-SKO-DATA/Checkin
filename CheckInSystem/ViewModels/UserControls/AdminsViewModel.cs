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
    public class AdminsViewModel : ViewModelBase
    {
        AdminUser adminUser = new();

        public string txtUsername {  get; set; }

        public string txtPassword { get; set; }

        public ReactiveCommand<Unit, Unit> Btn_AddAdmin { get; }

        public AdminsViewModel(IPlatform platform) : base(platform)
        { 
            Btn_AddAdmin = ReactiveCommand.Create(() => {adminUser.CreateUser(txtUsername, txtPassword); });
        }
    }
}
