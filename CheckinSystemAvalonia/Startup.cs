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
    private static IScreenImpl screenImpl;

    public static bool Run(IPlatform platform)
    {

        if (!EnsureDatabaseAvailable()) return false;

        AddAdmin();
        return true;
    }

    public static void OpenEmployeeOverview(IPlatform iplatform)
    {

        //Screen screenRetriver = new();

        SettingsControl settings = new SettingsControl();
        int screenIndex = settings.GetEmployeeOverViewSettings();

        var employeeOverviewViewModel = new EmployeeOverviewViewModel(iplatform);
        var employeeOverview = new EmployeeOverviewWindow(employeeOverviewViewModel)
        {
            DataTemplates = { new ViewLocator() },
            DataContext = employeeOverviewViewModel
        };
        if (false == true)
        { 
        }
        //var screensCount = screenRetriver.ScreenCount;
        //var screens = screenRetriver.All;
        /*
        if (screenIndex >= 0 && screenIndex < screensCount)
        {
            var targetScreen = screens[screenIndex];
            var bounds = targetScreen.Bounds;

            employeeOverview.WindowStartupLocation = Avalonia.Controls.WindowStartupLocation.Manual;
            employeeOverview.Position = new PixelPoint(bounds.X, bounds.Y);
            employeeOverview.Width = bounds.Width;
            employeeOverview.Height = bounds.Height;
        }*/
        else
        {
            employeeOverview.WindowStartupLocation = Avalonia.Controls.WindowStartupLocation.CenterScreen;
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
