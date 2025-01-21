using System.Windows;
using System.Windows.Controls;
using CheckInSystem.Models;
using CheckInSystem.ViewModels.UserControls;
using CheckInSystem.Views.Dialog;

namespace CheckInSystem.Views.UserControls;

public partial class AdminGroupView : UserControl
{
    private AdminGroupViewModel vm;
    public AdminGroupView()
    {
        vm = new AdminGroupViewModel();
        DataContext = vm;
        InitializeComponent();
    }

    private void BtnLogOut(object sender, RoutedEventArgs e)
    {
        vm.Logout();
    }

    private void BtnSwitchToGroups(object sender, RoutedEventArgs e)
    {
        vm.SwtichToEmployees();
    }

    private void BtnEditName(object sender, RoutedEventArgs e)
    {
        Button checkBox = (Button)sender;
        Group group = (Group)checkBox.DataContext;
        InputDialog input = new InputDialog("Indtast nyt navn til gruppen", group.Name);
        if (input.ShowDialog() == true)
        {
            if (input.Answer != "")
            {
                vm.EditGroupName(group, input.Answer);
            }
        }
    }

    private void BtnDeleteGroup(object sender, RoutedEventArgs e)
    {
        Button checkBox = (Button)sender;
        Group group = (Group)checkBox.DataContext;
        vm.DeleteGroup(group);
    }

    private void BtnCreateGroup(object sender, RoutedEventArgs e)
    {
        InputDialog input = new InputDialog("Indtast navn til gruppen");
        if (input.ShowDialog() == true)
        {
            if (input.Answer != "")
            {
                vm.AddNewGroup(input.Answer);
            }
        }
    }

    private void UpdateVisibility(object sender, RoutedEventArgs e)
    {
        CheckBox checkBox = (CheckBox)sender;
        Group group = (Group)checkBox.DataContext;
        group.Updatevisibility(group.Isvisible);
    }
}