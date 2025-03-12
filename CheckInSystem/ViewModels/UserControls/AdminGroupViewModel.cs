using CheckInSystem.Models;
using CheckInSystem.Platform;
using CheckInSystem.Views.UserControls;
using System.Collections.ObjectModel;

namespace CheckInSystem.ViewModels.UserControls;

public class AdminGroupViewModel : ViewModelBase
{
    ObservableCollection<Group> _groups = new();
    public ObservableCollection<Group> Groups
    {
        get => _groups;
        set => SetProperty(ref _groups, value, nameof(Groups));
    }
    public AdminGroupViewModel(IPlatform platform) : base(platform)
    {
        platform.DataLoaded += (sender, args) =>
        {
            Groups = platform.MainWindowViewModel.Groups;
            //foreach (var group in platform.MainWindowViewModel.Groups)
            //{
            //    group.Add(employee);
            //}
        };
    }
    
    public void Logout()
    {
        //MainContentControl.Content = new LoginScreen(new LoginScreenViewModel(_platform));
        _platform.MainWindowViewModel.RequestView(typeof(LoginScreen));
    }

    public void SwtichToEmployees()
    {
        //MainContentControl.Content = new AdminPanel(new AdminPanelViewModel(_platform,new()));
        _platform.MainWindowViewModel.RequestView(typeof(AdminPanel));
    }

    public void AddNewGroup(string name)
    {
        Groups.Add(Group.NewGroup(name));
    }

    public void EditGroupName(Group group, string name)
    {
        group.UpdateName(name, group.ID);
    }

    public void DeleteGroup(Group group)
    {
        group.RemoveGroupDb();
        Groups.Remove(group);
    }
}