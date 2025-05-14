using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using CheckinLibrary.Models;
using CheckInSystemAvalonia.Platform;
using CheckInSystemAvalonia.ViewModels.UserControls;

namespace CheckInSystemAvalonia.Views.UserControls;

public partial class AdminGroupView : UserControl
{
    public AdminGroupViewModel _vm => (AdminGroupViewModel)DataContext;
    public AdminGroupView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
    private void BtnEditName(object sender, RoutedEventArgs e)
    {
        Button checkBox = (Button)sender;
        Group group = (Group)checkBox.DataContext;
        InputDialog input = new InputDialog("Indtast nyt navn til gruppen", group.Name);

        _vm.EditGroupNameAsync(group, input);
    }

    private void BtnDeleteGroup(object sender, RoutedEventArgs e)
    {
        Button checkBox = (Button)sender;
        Group group = (Group)checkBox.DataContext;
        _vm.DeleteGroup(group);
    }
    private void UpdateVisibility(object sender, RoutedEventArgs e)
    {
        CheckBox checkBox = (CheckBox)sender;
        Group group = (Group)checkBox.DataContext;
        group.Updatevisibility(group.Isvisible);
    }
}