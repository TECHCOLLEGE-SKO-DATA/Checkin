namespace CheckinLibrary.Database;

using System;
using System.Configuration;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Threading;
using System.Windows;
using Microsoft.Win32;
using System.IO;
using System.Runtime.InteropServices;
using System.ServiceProcess;

public static class Database
{

    private const int CONNECTION_TIMEOUT = 30;
    private const int RETRY_ATTEMPTS = 3;
    private const int RETRY_DELAY_MS = 1000;

    private static string? _connectionString;
    private static string? _sqlServiceName;

    public static string ConnectionString
    {
        get
        {
            if (_connectionString == null)
            {
                _connectionString = ConfigurationManager.ConnectionStrings["CheckInSystemDb"]?.ConnectionString;
                if (string.IsNullOrEmpty(_connectionString))
                {
                    throw new Exception("Database connection string is missing in app.config.");
                }
            }
            return _connectionString;
        }
    }

    public static string SqlServiceName
    {
        get
        {
            if (_sqlServiceName == null)
            {
                _sqlServiceName = ConfigurationManager.AppSettings["SqlServiceName"];
                if (string.IsNullOrEmpty(_sqlServiceName))
                {
                    throw new Exception("SQL service name is missing in app.config.");
                }
            }
            return _sqlServiceName;
        }
    }

    public static bool EnsureDatabaseAvailable()
    {

        try
        {
            if (!IsSqlServerInstalled())
            {
                /*MessageBox.Show("Microsoft SQL Server er ikke installeret!",
                    "Database Fejl", MessageBoxButton.OK, MessageBoxImage.Error);*/
                return false;
            }

            if (!EnsureSqlServiceRunning())
            {
                /*MessageBox.Show("Kunne ikke starte SQL Serveren, kontakt IT support!",
                    "Database Fejl", MessageBoxButton.OK, MessageBoxImage.Error);*/
                return false;
            }

            return TestDatabaseConnection();
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);

            /*MessageBox.Show($"Database opstartsfejl: {e.Message}",
                "Database Fejl", MessageBoxButton.OK, MessageBoxImage.Error);*/
            return false;
        }
    }

    public static SqlConnection GetConnection()
    {
        SqlConnection connection = new SqlConnection(ConnectionString);

        for (int attempt = 1; attempt <= RETRY_ATTEMPTS; attempt++)
        {
            try
            {
                if (connection.State != System.Data.ConnectionState.Open)
                    connection.Open();

                return connection;
            }
            catch (SqlException e)
            {
                Debug.WriteLine(e);

                if (attempt == RETRY_ATTEMPTS)
                {
                    throw new Exception($"Failed to connect to database after {RETRY_ATTEMPTS} attempts: {e.Message}");
                }

                Thread.Sleep(RETRY_DELAY_MS);
            }
        }

        return null;
    }

    private static bool IsSqlServerInstalled()
    {
        return ServiceController
            .GetServices()
            .Any(sc => sc.ServiceName.Equals(SqlServiceName));
    }

    private static bool EnsureSqlServiceRunning()
    {
        try
        {
            using (ServiceController sc = new ServiceController(SqlServiceName))
            {
                if (sc.Status == ServiceControllerStatus.Running)
                    return true;

                if (sc.Status == ServiceControllerStatus.Stopped)
                {
                    sc.Start();
                    sc.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(30));
                }

                return sc.Status == ServiceControllerStatus.Running;
            }
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
            return false;
        }
    }

    private static bool TestDatabaseConnection()
    {
        try
        {
            using var connection = GetConnection();
            if (connection == null) return false;

            // Execute a simple query to test the connection.
            using (var command = new SqlCommand("SELECT 1", connection))
            {
                command.ExecuteScalar();
                return true;
            }
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
            return false;
        }
    }
}
