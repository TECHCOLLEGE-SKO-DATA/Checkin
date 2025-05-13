using Avalonia.Controls;
using Avalonia.Platform;
using CheckinLibrary.Models;
using CheckInSystemAvalonia.Platform;
using CheckInSystemAvalonia.ViewModels.Windows;
using CheckInSystemAvalonia.Views.UserControls;
using PCSC;
using PCSC.Interop;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace CheckInSystemAvalonia.ViewModels.UserControls
{
    public class AdminPanelViewModel : ViewModelBase
    {

        public const int EMPLOYEE_LISTPAGE_TAB = 0;
        public const int EMPLOYEE_TIME_TAB = 1;
        public const int GROUP_LISTPAGE_TAB = 2;

        private IPlatform _platform;

        public ObservableCollection<Group> Groups { get; private set; } = new();

        private Control _adminPanelContent;
        public Control AdminPanelContent
        {
            get => _adminPanelContent;
            set => this.RaiseAndSetIfChanged(ref _adminPanelContent, value);
        }

        AdminEmployeeViewModel _adminEmployeeViewModel;
        public AdminEmployeeViewModel AdminEmployeeViewModel
        {
            get => _adminEmployeeViewModel;
            set => SetProperty(ref _adminEmployeeViewModel, value, nameof(AdminEmployeeViewModel));
        }

        //public AdminEmployeeViewModel AdminEmployeeViewModel { get; }
        public AdminGroupViewModel AdminGroupViewModel { get; }
        public EmployeeTimeViewModel EmployeeTimeViewModel { get; }

        private int _selectedTab;
        public int SelectedTab
        {
            get => _selectedTab;
            set => this.RaiseAndSetIfChanged(ref _selectedTab, value);
        }

        // Add this field
        private Group _selectedGroup;
        public Group SelectedGroup
        {
            get => _selectedGroup;
            set => this.RaiseAndSetIfChanged(ref _selectedGroup, value);
        }

        // ReactiveCommands for actions
        public ReactiveCommand<Unit, Unit> EditGroupsForEmployeesCommand { get; }
        public ReactiveCommand<Unit, Unit> MarkAsOffsiteCommand { get; }
        public ReactiveCommand<Unit, Unit> DeleteEmployeesCommand { get; }
        public ReactiveCommand<Unit, Unit> EditNextScannedCardCommand { get; }
        public ReactiveCommand<Unit, Unit> ResetGroupCommand { get; }
        public ReactiveCommand<Unit, Unit> Btn_LoginView { get; }
        public ReactiveCommand<Unit, Unit> Btn_GroupView { get; }
        public ReactiveCommand<Unit, Unit> Btn_SettingsView { get; }
        public AdminPanelViewModel(IPlatform platform) : base(platform)
        {
            platform.DataLoaded += (sender, args) =>
            {
                Groups = platform.MainWindowViewModel.Groups;
            };

            AdminEmployeeViewModel = new(platform);

            Btn_LoginView = ReactiveCommand.Create(() => platform.MainWindowViewModel.SwitchToLoginView());
            Btn_GroupView = ReactiveCommand.Create(() => platform.MainWindowViewModel.SwitchToGroupView());
            Btn_SettingsView = ReactiveCommand.Create(() => platform.MainWindowViewModel.SwitchToSettingsView());
            EditGroupsForEmployeesCommand = ReactiveCommand.Create(EditGroupsForEmployees);
            MarkAsOffsiteCommand = ReactiveCommand.Create(() =>
            {
                // Replace with actual SelectedEmployees when reintroducing AdminEmployeeViewModel
                var selected = new ObservableCollection<Employee>();
                UpdateOffsite(selected, DateTime.Today, DateTime.Today.AddDays(1), "Orlov", Absence.absenceReason.Ferie);
            });

            DeleteEmployeesCommand = ReactiveCommand.Create(() =>
            {
                var selected = new ObservableCollection<Employee>();
                DeleteEmployee(selected);
            });

            EditNextScannedCardCommand = ReactiveCommand.Create(EditNextScannedCard);
           //ResetGroupCommand = ReactiveCommand.Create(() => SelectedGroup = null);

        }



        public void EditNextScannedCard()
        {
            //CardReader.State.UpdateNextEmployee = true;
            //Views.Dialog.WaitingForCardDialog.Open();
        }

        public void DeleteEmployee(Employee employee)
        {
            employee.DeleteFromDb();
            foreach (Group group in _platform.MainWindowViewModel.Groups)
            {
                group.Members.Remove(employee);
            }
            //_platform.MainWindowViewModel.Remove(employee);
        }

        public void DeleteEmployee(ObservableCollection<Employee> employees)
        {
            foreach (Employee employee in employees)
            {
                DeleteEmployee(employee);
            }
        }

        public void UpdateOffsite(Employee employee, bool isOffsite, DateTime? offsiteUntil)
        {
            employee.IsOffSite = isOffsite;
            employee.OffSiteUntil = offsiteUntil;
            employee.UpdateDb();
        }

        public void UpdateOffsite(ObservableCollection<Employee> employees,
            DateTime FromDate, DateTime ToDate, string Note, Absence.absenceReason AbsenceReason)
        {
            Absence absence = new();
            absence.ToTime = absence.ToTime.AddHours(23);
            foreach (Employee employee in employees)
            {
                absence.InsertAbsence(employee.ID, FromDate, ToDate, Note, AbsenceReason);
            }
        }

        public void AddSelectedUsersToGroup(Group group)
        {
            /*foreach (var employee in AdminEmployeeViewModel.SelectedEmployees)
            {
                group.AddEmployee(employee);
            }*/
        }

        public void RemoveSelectedUsersToGroup(Group group)
        {
            /*foreach (var employee in AdminEmployeeViewModel.SelectedEmployees)
            {
                group.RemoveEmployee(employee);
            }*/
        }

        public void EditGroupsForEmployees()
        {
            /*
            var editGroupsForEmployees = new EditGroupsForEmployees(_mainWindowViewModel.Groups);

            if (editGroupsForEmployees.ShowDialog() == true && editGroupsForEmployees.SelectedGroup != null)
            {
                if (editGroupsForEmployees.AddGroup)
                    AddSelectedUsersToGroup(editGroupsForEmployees.SelectedGroup);

                if (editGroupsForEmployees.RemoveGroup)
                    RemoveSelectedUsersToGroup(editGroupsForEmployees.SelectedGroup);
            }*/
        }
    }
}
