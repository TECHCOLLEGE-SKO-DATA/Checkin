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
using CheckinLib.Platform;

namespace CheckinSystemAvalonia;

public partial class App : Application
{
    private static IPlatform platform;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        LoadingStartup loadingStartup = new LoadingStartup();
        loadingStartup.Show();
        AppDomain.CurrentDomain.UnhandledException += log;
        try
        {
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
        if (!Directory.Exists(filePath))
        {
            Directory.CreateDirectory(filePath);
        }
        filePath += @"\log.txt";
        File.AppendAllText(filePath, $"At {DateTime.Now} {e}\r\n");
    }

    public bool OpenMessageBox(string tital, string errormessage)
    {
        MessageBoxWindow messageBoxWindow = new();
        MessageBoxViewModel messageBoxViewMode = new(platform ,tital, errormessage);
        messageBoxWindow.Show();
        return true;
    } 
}
