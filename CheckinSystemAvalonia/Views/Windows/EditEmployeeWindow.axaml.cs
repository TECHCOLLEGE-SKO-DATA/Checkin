using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using CheckinLibrary.Models;
using CheckInSystemAvalonia.Platform;
using CheckInSystemAvalonia.ViewModels.Windows; 

namespace CheckInSystemAvalonia.Views;

public partial class EditEmployeeWindow : Window
{
    EditEmployeeViewModel _vm { get => (EditEmployeeViewModel)DataContext; set => DataContext = value; }
    static IPlatform _platform;
    public EditEmployeeWindow(IPlatform platform, EditEmployeeViewModel viewModel)
    {
        _vm = viewModel;
        //this.DataContext = vm;
        _platform = platform;
        Closing += _vm.OnWindowClosing;
        InitializeComponent();
        Topmost = true;
        _vm.UpdateCardMessage = UpdateCardMessage;
    }

    private void UpdateCardId(object sender, RoutedEventArgs e)
    {
        _vm.UpdateCardId();
    }

    public static void Open(Employee employee)
    {
        Dispatcher.UIThread.Post(() => {
            var editWindow = new EditEmployeeWindow(_platform, new EditEmployeeViewModel(_platform, employee));
            editWindow.Show();
        });
    }

    private void Close(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}