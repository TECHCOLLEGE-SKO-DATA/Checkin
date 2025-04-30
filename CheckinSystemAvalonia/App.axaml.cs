using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using CheckinSystemAvalonia.ViewModels;
using CheckinSystemAvalonia.Views;
using CheckinSystemAvalonia;
using CheckinSystemAvalonia.ViewModels.Windows;
using CheckinLib;
using System;
using System.IO;
using CheckinSystemAvalonia.Platform;

namespace CheckinSystemAvalonia;

public partial class App : Application
{
    public static IPlatform Platform { get; private set; } = null!;

    public override void OnFrameworkInitializationCompleted()
    {
        AppDomain.CurrentDomain.UnhandledException += log;
        try
        {
            Platform = new Platform.Platform();

            LoadingStartup loadingStartup = new();
            loadingStartup.Show();

            if (!Startup.Run(Platform)) 
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

    public bool OpenMessageBox(string title, string errormessage)
    {
        var messageBoxWindow = new MessageBoxWindow
        {
            DataContext = new MessageBoxViewModel(Platform, title, errormessage)
        };
        messageBoxWindow.Show();
        return true;
    }

    private static void log(object sender, UnhandledExceptionEventArgs e)
    {
        string filePath = Environment.ExpandEnvironmentVariables(@"%AppData%\checkInSystem");
        if (!Directory.Exists(filePath)) Directory.CreateDirectory(filePath);
        File.AppendAllText(Path.Combine(filePath, "log.txt"), $"At {DateTime.Now} {e}\r\n");
    }
}
