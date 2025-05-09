using Avalonia.Controls;
using Avalonia.Platform;
using CheckinSystemAvalonia.Platform;
using CheckinSystemAvalonia.ViewModels.Windows;
using System.ComponentModel;

namespace CheckinSystemAvalonia.Views;

public partial class MainWindow : Window
{
    static readonly Platform.Platform platform = new();
    MainWindowViewModel _vm { get => (MainWindowViewModel)DataContext; set => DataContext = value; }

    public MainWindowViewModel MainWindowViewModel { get => _vm; set => _vm = value; }

    public MainWindow()
    {
        //platform.Start();
        InitializeComponent();

        _vm = platform.MainWindowViewModel;

        LoadingStartup loading = new();

        Closing += OnWindowClosing;
        LoadingStartup.Instance?.Close();

#if DEBUG
        OpenFakeNFCWindow();
#endif
    }
    public void OnWindowClosing(object sender, CancelEventArgs e)
    {
    }

    private static void OpenFakeNFCWindow()
    {

        var fakeNFCWindow = new FakeNFCWindow
        {
            DataTemplates = { new ViewLocator() },
            DataContext = new FakeNFCWindowViewModel(platform)
        };

        fakeNFCWindow.Show();
    }
}
