using Avalonia.Controls;
using Avalonia.Platform;
using Avalonia;
using CheckInSystemAvalonia;
using CheckinLibrary.Database;
using CheckinLibrary.Settings;
using CheckInSystemAvalonia.ViewModels.Windows;
using CheckInSystemAvalonia.Views;
using CheckInSystemAvalonia.Platform;

public class Startup
{
    public static bool Run()
    {

        if (!EnsureDatabaseAvailable()) return false;

        AddAdmin();
        return true;
    }

    public static void OpenEmployeeOverview(IPlatform iplatform)
    {
        Window window = new Window();

        SettingsControl settings = new SettingsControl();
        int screenIndex = settings.GetEmployeeOverViewSettings();

        var employeeOverviewViewModel = new EmployeeOverviewViewModel(iplatform);
        var employeeOverview = new EmployeeOverviewWindow(employeeOverviewViewModel)
        {
            DataTemplates = { new ViewLocator() },
            DataContext = employeeOverviewViewModel
        };

        var screens = window.Screens;

        if (screens is not null && screenIndex > 0 && screenIndex <= screens.All.Count)
        {
            //make sure the screenindex aligns with how it counts screens being 0 for screen 1 and 1 for screen 2
            screenIndex = screenIndex - 1;

            var targetScreen = screens.All[screenIndex];
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

    // Ensure database is available
    private static bool EnsureDatabaseAvailable()
    {
        if (!Database.EnsureDatabaseAvailable())
        {
            return false;
        }
        return true;
    }
}
