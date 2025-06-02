using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Collections.ObjectModel;
using CheckinLibrary.Models;
using Avalonia.Interactivity;

namespace CheckInSystemAvalonia;

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
        Close(true);
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