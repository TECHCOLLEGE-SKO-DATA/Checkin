namespace CheckInSystem.Models;

using System.Data.SqlClient;
using System.Diagnostics;
using CheckInSystem.Database;
using Dapper;
using BCrypt.Net;
using Database;

public class AdminUser
{
    public int ID { get; private set; }
    public string Username { get; private set; }
    public AdminUser()
    {

    }
    public AdminUser(string username)
    {
        Username = username;
        
    }
    
    public static void CreateUser(string username, string password)
    {
        DatabaseHelper.CreateUser(username, password);
    }

    public static AdminUser? Login(string username, string password)
    {
        return DatabaseHelper.Login(username, password);
    }

    public static List<AdminUser> GetAdminUsers()
    {
        return DatabaseHelper.GetAdminUsers();
    }
    public void Delete(int ID)
    {
        DatabaseHelper.Delete(ID);
    }
}
