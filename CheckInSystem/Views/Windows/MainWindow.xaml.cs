using System.ComponentModel;
using System.Windows;
using CheckInSystem.ViewModels;
using CheckInSystem.Views.UserControls;

namespace CheckInSystem;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        Closing += OnWindowClosing;
        ViewmodelBase.MainContentControl = MainContent;
        MainContent.Content = new LoginScreen();
    }
    
    public void OnWindowClosing(object sender, CancelEventArgs e)
    {
        System.Windows.Application.Current.Shutdown();
    }
}