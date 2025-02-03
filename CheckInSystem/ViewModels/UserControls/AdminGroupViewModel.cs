using CheckInSystem.Models;
using CheckInSystem.Platform;
using CheckInSystem.Views.UserControls;

namespace CheckInSystem.ViewModels.UserControls;

public class AdminGroupViewModel : ViewModelBase
{
    public AdminGroupViewModel(IPlatform platform) : base(platform)
    {
        
    }
    
    public void Logout()
    {
        MainContentControl.Content = new LoginScreen(new LoginScreenViewModel(_platform));
    }

    public void SwtichToEmployees()
    {
        MainContentControl.Content = new AdminPanel(new AdminPanelViewModel(_platform,new()));
    }

    public void AddNewGroup(string name)
    {
        Groups.Add(Group.NewGroup(name));
    }

    public void EditGroupName(Group group, string name)
    {
        group.UpdateName(name);
    }

    public void DeleteGroup(Group group)
    {
        group.RemoveGroupDb();
        Groups.Remove(group);
    }
}