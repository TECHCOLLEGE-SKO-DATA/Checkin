namespace CheckInSystem.Models;

using System.Data.SqlClient;
using System.Diagnostics;
using CheckInSystem.Database;
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
