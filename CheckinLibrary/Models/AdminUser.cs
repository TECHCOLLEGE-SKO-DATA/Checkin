namespace CheckinLibrary.Models;

using System.Data.SqlClient;
using System.Diagnostics;
using CheckinLibrary.Database;
using Dapper;
using BCrypt.Net;
using Database;

public class AdminUser
{
    DatabaseHelper databasehelper = new();
    public int ID { get; private set; }
    public string Username { get; private set; }
    public AdminUser()
    {
    }
    public AdminUser(string username)
    {
        Username = username;
    }

    public void UpdateUser(string NewUsername, string NewPassword, string OldUsername, string OldPassword)
    {
        if (OldUsername != "" && OldPassword != "")
        {
            var user = databasehelper.Login(OldUsername, OldPassword);
            if (user != null)
            {
                databasehelper.UpdateUser(NewUsername, NewPassword, user.ID);
            }
            else
            {
                Debug.WriteLine("Failed to Update user: User was Null/not found or returned");
            }
        }
        else if (NewUsername != null && NewPassword != null)
        {
            AdminUser adminUser = databasehelper.GetAdminUsers().First(x => x.Username == NewUsername);

            databasehelper.UpdateUser(NewUsername, NewPassword, adminUser.ID);
        }
        else
        {
            Debug.WriteLine("Failed to Update user: no new Username/Password");
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
