using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using CheckInSystem.CardReader;
using CheckInSystem.Database;
using CheckInSystem.Models;
using CheckInSystem.Platform;
using CheckInSystem.ViewModels;
using CheckInSystem.ViewModels.UserControls;
using CheckInSystem.ViewModels.Windows;
using CheckInSystem.Views;
using WpfScreenHelper;

namespace CheckInSystem;

public class Startup
{
    private static IPlatform _platform;

    public static bool Run()
    {
        DatabaseHelper dbHelper = new DatabaseHelper();
        
        if (!EnsureDatabaseAvailable()) return false;
        //ACR122U.StartReader();
        //ViewModelBase.Employees = new ObservableCollection<Employee>(dbHelper.GetAllEmployees());
        //ViewModelBase.Groups =
        //    new ObservableCollection<Group>(Group.GetAllGroups(new List<Employee>(ViewModelBase.Employees)));
        //OpenEmployeeOverview();
        AddAdmin();

        return true;   
    }

    //public static void OpenEmployeeOverview()
    //{
    //    var screens = Screen.AllScreens.GetEnumerator();
    //    screens.MoveNext();
    //    screens.MoveNext();
    //    Screen? screen = screens.Current;
    //    EmployeeOverview employeeOverview = new EmployeeOverview(new EmployeeOverviewViewModel(new WPFPlatform()));

    //    if (screen != null)
    //    {
    //        employeeOverview.Top = screen.Bounds.Top;
    //        employeeOverview.Left = screen.Bounds.Left;
    //        employeeOverview.Height = screen.Bounds.Height;
    //        employeeOverview.Width = screen.Bounds.Width;
    //    }
    //    employeeOverview.Show();
    //}
    public static void OpenEmployeeOverview(IPlatform _platform)
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