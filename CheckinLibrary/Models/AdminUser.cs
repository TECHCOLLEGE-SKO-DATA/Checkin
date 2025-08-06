namespace CheckinLibrary.Models;

using System.Data.SqlClient;
using System.Diagnostics;
using CheckinLibrary.Database;
using Dapper;
using BCrypt.Net;
using Database;

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

    public void UpdateUser(string username, string password)
    {
        var user = databasehelper.Login(username, password);
        if (user != null)
        {
            databasehelper.UpdateUser(username, password, user.ID);
        }
        else
        {
            Debug.WriteLine("Failed to Update user: User was Null/not found or returned");
        }
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
