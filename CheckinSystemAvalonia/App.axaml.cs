using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Primitives;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using CheckinLibrary;
using CheckInSystemAvalonia;
using CheckInSystemAvalonia.ViewModels;
using CheckInSystemAvalonia.ViewModels.Windows;
using CheckInSystemAvalonia.Views;
using System;
using System.IO;

namespace CheckInSystemAvalonia;

public partial class App : Application
{
    public static Platform.Platform Platform = new();

    public override void OnFrameworkInitializationCompleted()
    {
        AppDomain.CurrentDomain.UnhandledException += log;
        try
        {
            Platform.Start();

            BackGroundTheme(Platform.MainWindowViewModel.DarkMode);

            if (!Startup.Run())
            {
            }
        }
        catch (Exception exception)
        {
            Logger.LogError(exception);
            throw;
        }

        base.OnFrameworkInitializationCompleted();
    }

    public static void BackGroundTheme(bool darkMode)
    {
        if (Application.Current is { } app)
        {
            if (darkMode)
            {
                app.RequestedThemeVariant = ThemeVariant.Dark;
            }
            else
            {
                app.RequestedThemeVariant = ThemeVariant.Light;
            }
        }
    }

    private static void log(object sender, UnhandledExceptionEventArgs e)
    {
        string filePath = Environment.ExpandEnvironmentVariables(@"%AppData%\checkInSystem");
        if (!Directory.Exists(filePath)) Directory.CreateDirectory(filePath);
        File.AppendAllText(Path.Combine(filePath, "log.txt"), $"At {DateTime.Now} {e}\r\n");
    }
}