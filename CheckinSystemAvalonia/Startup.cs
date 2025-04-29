using Avalonia.Controls;
using Avalonia.Platform;
using Avalonia;
using CheckinSystemAvalonia;
using CheckinLib.Database;
using CheckinLib.Platform;
using CheckinLib.Settings;
using CheckinSystemAvalonia.ViewModels.Windows;
using CheckinSystemAvalonia.Views;

public class Startup
{
    private static IScreenImpl screenImpl;

    private static IPlatform platform;

    private static Platform _platform;

    public static bool Run()
    {
        DatabaseHelper dbHelper = new DatabaseHelper();

        if (!EnsureDatabaseAvailable()) return false;

        _platform = new Platform();
        platform = _platform;

        _platform.DataLoaded += (sender, args) => {};

        _platform.Start();

        var mainWindow = new MainWindow
        {
            DataTemplates = { new ViewLocator() },
            DataContext = new MainWindowViewModel(_platform)
        };
        mainWindow.Show();


        OpenEmployeeOverview(_platform);
        AddAdmin();

        return true;
    }

    public static void OpenEmployeeOverview(IPlatform iplatform)
    {
        platform = iplatform; 

        Screens screenRetriver = new Screens(screenImpl);

        SettingsControl settings = new SettingsControl();
        int screenIndex = settings.GetEmployeeOverViewSettings();

        var employeeOverviewViewModel = new EmployeeOverviewViewModel(_platform);
        var employeeOverview = new EmployeeOverviewWindow(employeeOverviewViewModel)
        {
            DataTemplates = { new ViewLocator() },
            DataContext = employeeOverviewViewModel
        };

        var screensCount = screenRetriver.ScreenCount;
        var screens = screenRetriver.All;

        if (screenIndex >= 0 && screenIndex < screensCount)
        {
            var targetScreen = screens[screenIndex];
            var bounds = targetScreen.Bounds;

            employeeOverview.WindowStartupLocation = Avalonia.Controls.WindowStartupLocation.Manual;
            employeeOverview.Position = new PixelPoint(bounds.X, bounds.Y);
            employeeOverview.Width = bounds.Width;
            employeeOverview.Height = bounds.Height;
        }
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
