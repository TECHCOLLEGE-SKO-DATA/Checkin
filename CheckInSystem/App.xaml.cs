using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Windows;
using CheckInSystem.Views.Dialog;

namespace CheckInSystem;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    void App_Startup(object sender, StartupEventArgs e)
    {
        LoadingStartup loadingStartup = new LoadingStartup();
        loadingStartup.Show();
        AppDomain.CurrentDomain.UnhandledException += log;
        try
        {
            if (!CheckInSystem.Startup.Run())
            {
                Current.Shutdown();
            }
            loadingStartup.Close();
        }
        catch (Exception exception)
        {
            Logger.LogError(exception);
            throw;
        }
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
    
}
