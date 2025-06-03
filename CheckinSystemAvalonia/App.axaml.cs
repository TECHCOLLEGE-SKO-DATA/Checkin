using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using CheckInSystemAvalonia.ViewModels;
using CheckInSystemAvalonia.Views;
using CheckInSystemAvalonia;
using CheckInSystemAvalonia.ViewModels.Windows;
using CheckinLibrary;
using System;
using System.IO;
using CheckInSystemAvalonia.Platform;
using CheckInSystemAvalonia.ViewModels.Windows;
using CheckInSystemAvalonia.Views;

namespace CheckInSystemAvalonia;

public partial class App : Application
{
    public static Platform.Platform Platform = new();

    public override void OnFrameworkInitializationCompleted()
    {
        AppDomain.CurrentDomain.UnhandledException += log;
        try
        {
            LoadingStartup loadingStartup = new();
            loadingStartup.Show();

            Platform.Start();

            if (!Startup.Run())
            {
            }

            loadingStartup.Close();
        }
        catch (Exception exception)
        {
            Logger.LogError(exception);
            throw;
        }

        base.OnFrameworkInitializationCompleted();
    }

    private static void log(object sender, UnhandledExceptionEventArgs e)
    {
        string filePath = Environment.ExpandEnvironmentVariables(@"%AppData%\checkInSystem");
        if (!Directory.Exists(filePath)) Directory.CreateDirectory(filePath);
        File.AppendAllText(Path.Combine(filePath, "log.txt"), $"At {DateTime.Now} {e}\r\n");
    }
}