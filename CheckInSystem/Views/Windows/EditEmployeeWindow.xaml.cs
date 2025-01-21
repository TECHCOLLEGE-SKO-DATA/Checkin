using System.Windows;
using CheckInSystem.Models;
using CheckInSystem.ViewModels.Windows;

namespace CheckInSystem.Views.Windows;

public partial class EditEmployeeWindow : Window
{
    public EditEmployeeViewModel vm;
    public EditEmployeeWindow(EditEmployeeViewModel viewModel)
    {
        vm = viewModel;
        this.DataContext = vm;
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
            EditEmployeeWindow editWindow = new EditEmployeeWindow(new EditEmployeeViewModel(employee));
            editWindow.Show();
        });
    }

    private void Close(object sender, RoutedEventArgs e)
    {
       Close();
    }
}