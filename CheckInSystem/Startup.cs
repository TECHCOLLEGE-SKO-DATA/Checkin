using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using CheckInSystem.CardReader;
using CheckInSystem.Database;
using CheckInSystem.Models;
using CheckInSystem.ViewModels;
using CheckInSystem.Views;
using WpfScreenHelper;

namespace CheckInSystem;

public class Startup
{
    public static bool Run()
    {
        if (!EnsureDatabaseAvailable()) return false;
        ACR122U.StartReader();
        ViewModelBase.Employees = new ObservableCollection<Employee>(DatabaseHelper.GetAllEmployees());
        ViewModelBase.Groups =
            new ObservableCollection<Group>(Group.GetAllGroups(new List<Employee>(ViewModelBase.Employees)));
        OpenEmployeeOverview();
        AddAdmin();
        Database.Maintenance.CheckOutEmployeesIfTheyForgot();
        Database.Maintenance.CheckForEndedOffSiteTime();
        return true;
    }

    private static void OpenEmployeeOverview()
    {
        var screens = Screen.AllScreens.GetEnumerator();
        screens.MoveNext();
        screens.MoveNext();
        Screen? screen = screens.Current;
        EmployeeOverview employeeOverview = new EmployeeOverview();

        if (screen != null)
        {
            employeeOverview.Top = screen.Bounds.Top;
            employeeOverview.Left = screen.Bounds.Left;
            employeeOverview.Height = screen.Bounds.Height;
            employeeOverview.Width = screen.Bounds.Width;
        }
        employeeOverview.Show();
    }

    private static void AddAdmin() //Needs to be updated at somepoint
    {
        var admins = AdminUser.GetAdminUsers();
        if (admins.Count == 0)
        {

            AdminUser.CreateUser("sko", "test123");
        }
    }

    private static bool EnsureDatabaseAvailable()
    {
        if (!Database.Database.EnsureDatabaseAvailable())
        {
            MessageBox.Show("Kunne ikke oprette forbindelse til databasen!",
                "Uforventet Fejl", MessageBoxButton.OK, MessageBoxImage.Error);

            return false;
        }
        return true;
    }
}