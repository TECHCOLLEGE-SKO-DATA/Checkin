using Avalonia.Controls.Primitives;
using CheckinLibrary.Database;
using CheckinLibrary.Models;
using CheckInSystem.Platform;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;

namespace CheckInSystem.ViewModels.UserControls
{
    public class AdminEmployeeOverviewViewModel : ViewModelBase
    {
        public DateTime date {  get; set; }

        public Popup Damn { get; set; } = new Popup();

        public List<AbsenceReason> AbsenceReasons { get; set; } = new ();

        Absence absenc = new();

        public ObservableCollection<Employee> Employees { get; set; }

        //button bindindgs
        public ReactiveCommand<Unit, Unit> Btn_Logout {  get; }

        public ReactiveCommand<Unit, Unit> Btn_Back {  get; }
        
        //will be used to open popup where you can select a date and press yes it will get every employee that checked in
        //that date and if they have futuer/current/previous day absence
        public ReactiveCommand<Unit, Unit> Btn_ChangeDate { get; }

        public ReactiveCommand<Unit,Unit> Btn_SaveChange {  get; }

        public AdminEmployeeOverviewViewModel(IPlatform platform):base(platform) 
        {
            _platform.DataLoaded += (sender, args) =>
            {
                
            };

            Btn_Logout = ReactiveCommand.Create(() => { _platform.MainWindowViewModel.SwitchToLoginView(); });

            Btn_Back = ReactiveCommand.Create(() => { _platform.MainWindowViewModel.SwitchToAdminPanel(); });

            Btn_ChangeDate = ReactiveCommand.Create(() => 
            {
                
            });

            Btn_SaveChange = ReactiveCommand.Create(() => { });
        }
    }
}