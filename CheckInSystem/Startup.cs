using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using CheckInSystem.CardReader;
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
        ViewmodelBase.Employees = new ObservableCollection<Employee>(Employee.GetAllEmployees());
        ViewmodelBase.Groups =
            new ObservableCollection<Group>(Group.GetAllGroups(new List<Employee>(ViewmodelBase.Employees)));
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

    private static void AddAdmin()
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