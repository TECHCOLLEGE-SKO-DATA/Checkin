namespace CheckinSystemAvalonia.Models;

using CheckinSystemAvalonia.Database;
using System.Collections.Generic;

public class AdminUser
{
    DatabaseHelper databasehelper;
    public int ID { get; private set; }
    public string Username { get; private set; }
    public AdminUser()
    {
    }
    public AdminUser(string username)
    {
        Username = username;
    }
    
    public void CreateUser(string username, string password)
    {
        databasehelper.CreateUser(username, password);
    }

    public AdminUser? Login(string username, string password)
    {
        return databasehelper.Login(username, password);
    }

    public List<AdminUser> GetAdminUsers()
    {
        return databasehelper.GetAdminUsers();
    }
    public void Delete(int ID)
    {
        databasehelper.Delete(ID);
    }
}
