using System.Collections.ObjectModel;
using System.Windows;
using CheckInSystem.Models;

namespace CheckInSystem.Views.Dialog;

public partial class EditGroupsForEmployees : Window
{
    public ObservableCollection<Group> Groups { get; set; }
    public Group? SelectedGroup { get; set; }
    public EditGroupsForEmployees(ObservableCollection<Group> groups)
    {
        Groups = groups;
        InitializeComponent();
        DataContext = this;
    }
    
    private void BtnConfrimed(object sender, RoutedEventArgs e)
    {
        this.DialogResult = true;
    }

    public bool AddGroup
    {
        get => RdAddGroup.IsChecked ?? false;
    }

    public bool RemoveGroup
    {
        get => RdRemoveGroup.IsChecked ?? false;
    }
}