using CheckinLibrary.Models;
using CheckInSystemAvalonia.Controls;
using CheckInSystemAvalonia.Platform;
using CheckInSystemAvalonia.ViewModels.Windows;
using CheckInSystemAvalonia.Views;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckInSystemAvalonia.ViewModels.UserControls
{
    public class AdminEmployeeViewModel : ViewModelBase
    {
        private readonly AdminPanelViewModel _adminPanelViewModel;

        ObservableCollection<Employee> _selectedEmployeeGroup = new();
        public ObservableCollection<Employee> SelectedEmployeeGroup
        {
            get => _selectedEmployeeGroup;
            set => SetProperty(ref _selectedEmployeeGroup, value, nameof(SelectedEmployeeGroup));
        }
        public static ObservableCollection<Employee> SelectedEmployees { get; set; }

        public AdminEmployeeViewModel(IPlatform platform, AdminPanelViewModel adminPanelViewModel) : base(platform)
        {
            _adminPanelViewModel = adminPanelViewModel;

            // Reactive update whenever selected group changes
            adminPanelViewModel.WhenAnyValue(x => x.SelectedGroup)
                .Where(group => group != null)
                .Subscribe(group =>
                {
                    var sortedList = group.Members.OrderBy(emp => emp.FirstName).ToList();
                    SelectedEmployeeGroup.Clear();
                    foreach (var employee in sortedList)
                    {
                        SelectedEmployeeGroup.Add(employee);
                    }
                });

            SelectedEmployees = new();
        }


        public void EditEmployee(Employee employee)
        {
            EditEmployeeWindow.Open(employee, _platform);
        }

        public void DeleteEmployee(Employee employee)
        {
            employee.DeleteFromDb();
            foreach (var group in _platform.MainWindowViewModel.Groups)
            {
                group.Members.Remove(employee);
            }
            _platform.MainWindowViewModel.Employees.Remove(employee);
        }

        public void SeeEmployeeTime(Employee employee)
        {
            _platform.MainWindowViewModel.SwitchToEmployeeTime(employee);
        }

        public async Task OpenMessageBoxDeleteAsync(Employee employee)
        {
            var result = await MessageBox.Show(
            _platform.MainWindow,
            $"Er du sikker på at du vil slette {employee.FirstName} {employee.MiddleName} {employee.LastName}?",
            "Sletning",
            MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                DeleteEmployee(employee);

                _platform.MainWindowViewModel.AdminPanelViewModel.UpdateGroupAll();
            }
        }

        

        public async Task EditEmployeeGroup(Employee employee)
        {
            EditGroupsForEmployees editGroupsForEmployees = new(_platform.MainWindowViewModel.Groups);
            var result = await editGroupsForEmployees.ShowDialog<bool>(_platform.MainWindow);
            if (result == true && editGroupsForEmployees.SelectedGroup != null)
            {
                if (editGroupsForEmployees.AddGroup) editGroupsForEmployees.SelectedGroup.AddEmployee(employee);
                if (editGroupsForEmployees.RemoveGroup) editGroupsForEmployees.SelectedGroup.RemoveEmployee(employee);
            }
        }
        
    }
}
