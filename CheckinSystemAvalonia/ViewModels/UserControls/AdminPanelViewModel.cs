using Avalonia.Controls;
using Avalonia.Platform;
using CheckinLibrary.Models;
using CheckInSystemAvalonia.Platform;
using CheckInSystemAvalonia.ViewModels.Windows;
using CheckInSystemAvalonia.Views.UserControls;
using DynamicData;
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

        public ObservableCollection<Group> Groups { get; private set; } = new();

        private Control _adminPanelContent;
        public Control AdminPanelContent
        {
            get => _adminPanelContent;
            set => this.RaiseAndSetIfChanged(ref _adminPanelContent, value);
        }

        AdminEmployeeViewModel _adminEmployeeViewModel;
        public AdminEmployeeViewModel adminEmployeeViewModel
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


        private Group _selectedGroup;
        public Group SelectedGroup
        {
            get => _selectedGroup;
            set => this.RaiseAndSetIfChanged(ref _selectedGroup, value);
        }

        public ObservableCollection<Group> GroupsWithAll
        {
            get
            {
                var withAll = new ObservableCollection<Group>();

                // Create "All" group without affecting core Groups collection
                var allGroup = new Group
                {
                    Name = "All",
                    Isvisible = false
                };

                // Optionally populate members
                var allMembers = Groups
                    .SelectMany(g => g.Members)
                    .GroupBy(e => e.ID)
                    .Select(g => g.First())
                    .ToList();

                allGroup.InitializeMembers(allMembers);

                withAll.Add(allGroup);

                foreach (var group in Groups)
                {
                    withAll.Add(group);
                }

                return withAll;
            }
        }

        // ReactiveCommands for actions
        public ReactiveCommand<Unit, Unit> EditGroupsForEmployeesCommand { get; }
        public ReactiveCommand<Unit, Unit> MarkAsOffsiteCommand { get; }
        public ReactiveCommand<Unit, Unit> DeleteEmployeesCommand { get; }
        public ReactiveCommand<Unit, Unit> EditNextScannedCardCommand { get; }
        public ReactiveCommand<Unit, Unit> Btn_LoginView { get; }
        public ReactiveCommand<Unit, Unit> Btn_GroupView { get; }
        public ReactiveCommand<Unit, Unit> Btn_SettingsView { get; }

        public AdminPanelViewModel(IPlatform platform) : base(platform)
        {
            platform.DataLoaded += (sender, args) =>
            {
                Groups = platform.MainWindowViewModel.Groups;
                SelectedGroup = GroupsWithAll.FirstOrDefault();
            };

            adminEmployeeViewModel = new(platform, this);

            Btn_LoginView = ReactiveCommand.Create(() => platform.MainWindowViewModel.SwitchToLoginView());
            Btn_GroupView = ReactiveCommand.Create(() => platform.MainWindowViewModel.SwitchToGroupView());
            Btn_SettingsView = ReactiveCommand.Create(() => platform.MainWindowViewModel.SwitchToSettingsView());

            EditGroupsForEmployeesCommand = ReactiveCommand.Create(EditGroupsForEmployeesAsync);

            MarkAsOffsiteCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                var editOffsiteDialog = new EditOffsiteDialog(
                         _platform.MainWindowViewModel.absenceReasons.ToList(),
                         selectedReason: _platform.MainWindowViewModel.absenceReasons.FirstOrDefault(r => r.Reason == "Ferie")
                     );


                var result = await editOffsiteDialog.ShowDialog<bool>(platform.MainWindow);

                if (result)
                {
                    var fromDate = editOffsiteDialog.FromDate ?? DateTime.Today;
                    var toDate = editOffsiteDialog.ToDate ?? DateTime.Today;
                    var note = editOffsiteDialog.Note ?? string.Empty;
                    var reason = editOffsiteDialog.AbsenceReason ?? platform.MainWindowViewModel.absenceReasons.FirstOrDefault(r => r.Reason == "Ferie");                    // Fallback if none selected

                    UpdateOffsite(AdminEmployeeViewModel.SelectedEmployees, fromDate.Date, toDate.Date, note, reason);
                }
            });


            DeleteEmployeesCommand = ReactiveCommand.Create(() =>
            {
                DeleteEmployee(AdminEmployeeViewModel.SelectedEmployees);
            });

            EditNextScannedCardCommand = ReactiveCommand.Create(EditNextScannedCard);
           //ResetGroupCommand = ReactiveCommand.Create(() => SelectedGroup = null);

        }



        public void EditNextScannedCard()
        {
            CardReader.State.UpdateNextEmployee = true;
            WaitingForCardDialog.Open();
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
                _platform.MainWindowViewModel.Employees.Remove(employee);
            }
        }

        public void UpdateOffsite(Employee employee, bool isOffsite, DateTime? offsiteUntil)
        {
            employee.IsOffSite = isOffsite;
            employee.OffSiteUntil = offsiteUntil;
            employee.UpdateDb();
        }

        public void UpdateOffsite(ObservableCollection<Employee> employees,
            DateTime FromDate, DateTime ToDate, string Note, AbsenceReason AbsenceReason)
        {
            Absence absence = new();
            absence.ToTime = absence.ToTime.AddHours(23);
            foreach (Employee employee in employees)
            {
                absence.InsertAbsence(employee.ID, FromDate, ToDate, Note, AbsenceReason.Id);
            }
        }

        public void AddSelectedUsersToGroup(Group group)
        {
            foreach (var employee in AdminEmployeeViewModel.SelectedEmployees)
            {
                group.AddEmployee(employee);
            }
        }

        public void RemoveSelectedUsersToGroup(Group group)
        {
            foreach (var employee in AdminEmployeeViewModel.SelectedEmployees)
            {
                group.RemoveEmployee(employee);
            }
        }

        public async void EditGroupsForEmployeesAsync()
        {
            EditGroupsForEmployees editGroupsForEmployees = new(_platform.MainWindowViewModel.Groups);
            var result = await editGroupsForEmployees.ShowDialog<bool>(_platform.MainWindow);
            if (result == true && editGroupsForEmployees.SelectedGroup != null)
            {
                if (editGroupsForEmployees.AddGroup)
                    AddSelectedUsersToGroup(editGroupsForEmployees.SelectedGroup);

                if (editGroupsForEmployees.RemoveGroup)
                    RemoveSelectedUsersToGroup(editGroupsForEmployees.SelectedGroup);
            }
        }
    }
}
