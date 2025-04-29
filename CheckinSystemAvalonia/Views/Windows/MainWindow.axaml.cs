using Avalonia.Controls;
using Avalonia.Platform;
using CheckinLib.Platform;
using CheckinSystemAvalonia.ViewModels.Windows;
using System.ComponentModel;

namespace CheckinSystemAvalonia.Views;

public partial class MainWindow : Window
{
    static IPlatform platform;
    MainWindowViewModel _vm { get => (MainWindowViewModel)DataContext; set => DataContext = value; }

    public MainWindowViewModel MainWindowViewModel { get => _vm; set => _vm = value; }

    public MainWindow()
    {

        InitializeComponent();

        //_vm = platform.MainWindowViewModel;

        LoadingStartup loading = new();
        loading.Close();

#if DEBUG
        OpenFakeNFCWindow();
#endif
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
