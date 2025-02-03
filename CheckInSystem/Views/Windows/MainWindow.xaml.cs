using System.ComponentModel;
using System.Windows;
using CheckInSystem.CardReader;
using CheckInSystem.Database;
using CheckInSystem.Models;
using CheckInSystem.Platform;
using CheckInSystem.ViewModels;
using CheckInSystem.ViewModels.UserControls;
using CheckInSystem.Views.UserControls;

namespace CheckInSystem;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    WPFPlatform platform = new WPFPlatform();
    public MainWindow()
    {
        InitializeComponent();
        Closing += OnWindowClosing;
        ViewModelBase.MainContentControl = MainContent;
        MainContent.Content = new LoginScreen(new LoginScreenViewModel(platform));
    }
    
    public void OnWindowClosing(object sender, CancelEventArgs e)
    {
        System.Windows.Application.Current.Shutdown();
    }
}