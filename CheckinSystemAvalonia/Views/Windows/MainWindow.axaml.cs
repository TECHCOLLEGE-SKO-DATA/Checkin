using Avalonia.Controls;
using Avalonia.Platform;
using CheckInSystemAvalonia.Platform;
using CheckInSystemAvalonia.ViewModels.Windows;
using System.ComponentModel;

namespace CheckInSystemAvalonia.Views;

public partial class MainWindow : Window
{
    MainWindowViewModel _vm { get => (MainWindowViewModel)DataContext; set => DataContext = value; }

    public MainWindowViewModel MainWindowViewModel { get => _vm; set => _vm = value; }

    public MainWindow(IPlatform platform)
    {
        //platform.Start();
        InitializeComponent();

        _vm = platform.MainWindowViewModel;

        LoadingStartup loading = new();

        Closing += OnWindowClosing;
        LoadingStartup.Instance?.Close();

#if DEBUG
        OpenFakeNFCWindow(platform);
#endif
    }
    public void OnWindowClosing(object sender, CancelEventArgs e)
    {
    }

    private static void OpenFakeNFCWindow(IPlatform platform)
    {

        var fakeNFCWindow = new FakeNFCWindow
        {
            DataTemplates = { new ViewLocator() },
            DataContext = new FakeNFCWindowViewModel(platform)
        };

        fakeNFCWindow.Show();
    }
}
