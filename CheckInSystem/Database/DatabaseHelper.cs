namespace CheckInSystem.Database;

using CheckInSystem.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;

public class DatabaseHelper
{
    public static void CreateUser(string username, string password)
    {
        string insertQuery = @"INSERT INTO adminUser (username, hashedPassword) VALUES (@username, @passwordHash)";

        string passwordHash = BCrypt.EnhancedHashPassword(password);
        Debug.WriteLine(passwordHash);

        using var connection = Database.GetConnection();
        if (connection == null)
            throw new Exception("Could not establish database connection!");

        connection.Query(insertQuery, new { username = username, passwordHash = passwordHash });
    }

    public static AdminUser? Login(string username, string password)
    {
        string passwordHashQuery = @"SELECT hashedPassword FROM adminUser WHERE username = @username";
        string selectQuery = @"SELECT ID, username FROM adminUser WHERE username = @username";

        using var connection = Database.GetConnection();
        if (connection == null)
            throw new Exception("Could not establish database connection!");

        string? hashedPassword = connection.Query<string>(passwordHashQuery, new { username = username }).FirstOrDefault();
        if (hashedPassword == null) return null;
        if (!BCrypt.EnhancedVerify(password, hashedPassword)) return null;

        var adminUser = connection.Query<AdminUser>(selectQuery, new { username = username }).FirstOrDefault();
        return adminUser;
    }

    public static List<AdminUser> GetAdminUsers()
    {
        string selectQuery = @"SELECT * FROM adminUser";

        using var connection = Database.GetConnection();
        if (connection == null)
            throw new Exception("Could not establish database connection!");

        var adminUsers = connection.Query<AdminUser>(selectQuery).ToList();
        return adminUsers;
    }

    public static void Delete(int ID)
    {
        string deletionQuery = @"DELETE FROM adminUser WHERE ID = @id";

        using var connection = Database.GetConnection();
        if (connection == null)
            throw new Exception("Could not establish database connection!");

        connection.Query(deletionQuery, new { id = ID });
    }
}
