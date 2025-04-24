using System.Collections.ObjectModel;
using System.Diagnostics;
using CheckinSystemAvalonia.CardReader;
using CheckinSystemAvalonia.Database;
using CheckinSystemAvalonia.Settings;
using CheckinSystemAvalonia.Models;
using CheckinSystemAvalonia.Platform;
using CheckinSystemAvalonia.ViewModels;
using CheckinSystemAvalonia.ViewModels.UserControls;
using CheckinSystemAvalonia.ViewModels.Windows;
using CheckinSystemAvalonia.Views;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia;
using System.Threading.Tasks;
using Avalonia.Platform;
using System.Linq;

namespace CheckinSystemAvalonia;

public static class Startup
{
    private static IPlatform _platform;

    public static bool Run()
    {
        DatabaseHelper dbHelper = new DatabaseHelper();

        if (!EnsureDatabaseAvailable()) return false;

        //ACR122U.StartReader(); // Optional: uncomment if needed
        //ViewModelBase.Employees = new ObservableCollection<Employee>(dbHelper.GetAllEmployees());
        //ViewModelBase.Groups = new ObservableCollection<Group>(Group.GetAllGroups(new List<Employee>(ViewModelBase.Employees)));

        AddAdmin();

        return true;
    }

    public static void OpenEmployeeOverview(IPlatform platform)
    {
        _platform = platform;

        SettingsControl settings = new SettingsControl();
        int screenIndex = settings.GetEmployeeOverViewSettings();

        var employeeOverview = new EmployeeOverviewWindow(new EmployeeOverviewViewModel(_platform));

        var screens = employeeOverview.Screens.All.ToList();

        if (screenIndex >= 0 && screenIndex < screens.Count)
        {
            var targetScreen = screens[screenIndex];
            var bounds = targetScreen.Bounds;

            employeeOverview.WindowStartupLocation = WindowStartupLocation.Manual;
            employeeOverview.Position = new PixelPoint(bounds.X, bounds.Y);
            employeeOverview.Width = bounds.Width;
            employeeOverview.Height = bounds.Height;
        }
        else
        {
            employeeOverview.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        employeeOverview.Show();
    }

    private static void AddAdmin()
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
            var messageBox = new MessageBoxWindow
            {
                DataContext = new MessageBoxViewModel(
                    "Uforventet Fejl",
                    "Kunne ikke oprette forbindelse til databasen!")
            };

            var lifetime = Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;
            if (lifetime?.MainWindow != null)
            {
                messageBox.ShowDialog(lifetime.MainWindow);
            }
            else
            {
                
            }

            return false;
        }

        return true;
    }

}
