using CheckInSystem.Models;
using CheckInSystem.Views.UserControls;

namespace CheckInSystem.ViewModels.UserControls;

public class AdminGroupViewModel : ViewmodelBase
{
    public AdminGroupViewModel()
    {
        
    }
    
    public void Logout()
    {
        MainContentControl.Content = new LoginScreen();
    }

    public void SwtichToEmployees()
    {
        MainContentControl.Content = new AdminPanel();
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