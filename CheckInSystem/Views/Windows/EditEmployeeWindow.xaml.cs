using System.Windows;
using CheckInSystem.Models;
using CheckInSystem.Platform;
using CheckInSystem.ViewModels.UserControls;
using CheckInSystem.ViewModels.Windows;

namespace CheckInSystem.Views.Windows;

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
        Application.Current.Dispatcher.Invoke( () => {
            EditEmployeeWindow editWindow = new EditEmployeeWindow(_platform, new EditEmployeeViewModel(_platform, employee));
            editWindow.Show();
        });
    }

    private void Close(object sender, RoutedEventArgs e)
    {
       Close();
    }
}