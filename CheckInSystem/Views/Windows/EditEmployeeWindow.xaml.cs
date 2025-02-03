using System.Windows;
using CheckInSystem.Models;
using CheckInSystem.Platform;
using CheckInSystem.ViewModels.UserControls;
using CheckInSystem.ViewModels.Windows;

namespace CheckInSystem.Views.Windows;

public partial class EditEmployeeWindow : Window
{
    public EditEmployeeViewModel vm => (EditEmployeeViewModel)DataContext;
    static IPlatform _platform;
    public EditEmployeeWindow(IPlatform platform, EditEmployeeViewModel viewModel)
    {
        //vm = viewModel;
        //this.DataContext = vm;
        _platform = platform;
        Closing += vm.OnWindowClosing;
        InitializeComponent();
        Topmost = true;
        vm.UpdateCardMessage = UpdateCardMessage;
    }
    
    private void UpdateCardId(object sender, RoutedEventArgs e)
    {
        vm.UpdateCardId();
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