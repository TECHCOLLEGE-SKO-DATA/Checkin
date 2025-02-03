using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using CheckInSystem.CardReader;
using CheckInSystem.Database;
using CheckInSystem.Models;
using CheckInSystem.Platform;
using CheckInSystem.ViewModels;
using CheckInSystem.ViewModels.Windows;
using CheckInSystem.Views;
using WpfScreenHelper;

namespace CheckInSystem;

public class Startup
{

    public static bool Run()
    {
        DatabaseHelper dbHelper = new DatabaseHelper();
        
        if (!EnsureDatabaseAvailable()) return false;
        //ACR122U.StartReader();
        ViewModelBase.Employees = new ObservableCollection<Employee>(dbHelper.GetAllEmployees());
        ViewModelBase.Groups =
            new ObservableCollection<Group>(Group.GetAllGroups(new List<Employee>(ViewModelBase.Employees)));
        OpenEmployeeOverview();
        AddAdmin();
#if DEBUG
        OpenFakeNFCWindow();
#endif
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
        EmployeeOverview employeeOverview = new EmployeeOverview(new EmployeeOverviewViewModel(_platform));

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
        DatabaseHelper databaseHelper = new();
        var admins = databaseHelper.GetAdminUsers();
        if (admins.Count == 0)
        {

            databaseHelper.CreateUser("sko", "test123");
        }
    }
    private static void OpenFakeNFCWindow()
    {

        var screens = Screen.AllScreens.GetEnumerator();
        screens.MoveNext();
        Screen? screen = screens.Current;


        FakeNFCWindow fakeNFCWindow = new FakeNFCWindow();

        if (screen != null)
        {
            fakeNFCWindow.Top = screen.Bounds.Top;
            fakeNFCWindow.Left = screen.Bounds.Left;
            fakeNFCWindow.Height = 450;
            fakeNFCWindow.Width = 800;
        }

        fakeNFCWindow.Show();
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