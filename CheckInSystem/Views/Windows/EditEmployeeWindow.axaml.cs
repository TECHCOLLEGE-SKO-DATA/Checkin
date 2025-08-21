using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using CheckinLibrary.Models;
using CheckInSystem.Platform;
using CheckInSystem.ViewModels.Windows; 

namespace CheckInSystem.Views;

public partial class EditEmployeeWindow : Window
{
    EditEmployeeViewModel _vm { get => (EditEmployeeViewModel)DataContext; set => DataContext = value; }

    public EditEmployeeWindow(IPlatform platform, EditEmployeeViewModel viewModel)
    {
        _vm = viewModel;
        //this.DataContext = vm;
        Closing += _vm.OnWindowClosing;
        InitializeComponent();
        Topmost = true;
        _vm.UpdateCardMessage = UpdateCardMessage;
    }

    private void UpdateCardId(object sender, RoutedEventArgs e)
    {
        _vm.UpdateCardId();
    }

    public static void Open(Employee employee, IPlatform platform)
    {
        Dispatcher.UIThread.Post(() => {
            var editWindow = new EditEmployeeWindow(platform, new EditEmployeeViewModel(platform, employee));
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