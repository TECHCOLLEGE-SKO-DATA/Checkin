using CheckinLibrary.Background_tasks;
using CheckinLibrary.Database;
using CheckinLibrary.Models;
using CheckInSystem.Platform;
using CheckInSystem.ViewModels.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
public class BackgroundTimeService
{
    private readonly IDatabaseHelper _dbHelper;
    AbsencBackGroundService absence = new();

    private readonly TimeSpan _checkInterval = TimeSpan.FromMinutes(2);
    private readonly TimeSpan _startTime = new TimeSpan(1, 0, 0);  // 01:00 
    private readonly TimeSpan _endTime = new TimeSpan(4, 0, 0);    // 04:00 

    private bool _hasLoggedToday = true;
    private CancellationTokenSource _cts;
    private readonly Func<DateTime> _timeProvider;
    
    public event Action OnDailyReset;
    public Action<ObservableCollection<Employee>> PerformMaintenanceAction { get; set; } = _ => { };

    public Func<ObservableCollection<Employee>> GetEmployees { get; set; }


    public BackgroundTimeService(Func<DateTime> timeProvider = null, IDatabaseHelper dbHelper = null)
    {
        _timeProvider = timeProvider ?? (() => DateTime.Now);
    }

    public void Start(IPlatform platform)
    {
        this.PerformMaintenanceAction = (employees) =>
        {
            Maintenance.CheckOutEmployeesIfTheyForgot(employees.ToList());
            Maintenance.CheckForEndedOffSiteTime(employees.ToList());
        };

        _cts = new CancellationTokenSource();
        Task.Run(async () =>
        {
            while (!_cts.Token.IsCancellationRequested)
            {
                CheckTime(platform);
                await Task.Delay(_checkInterval);
            }
        });
    }

    public void Stop()
    {
        _cts?.Cancel();
    }

    public void CheckTime(IPlatform platform)
    {
        var currentTime = _timeProvider().TimeOfDay;

        if (currentTime >= _startTime && currentTime < _endTime && !_hasLoggedToday)
        {
            _hasLoggedToday = true;

            var employees = GetEmployees?.Invoke();
            if (employees != null)
            {
                PerformMaintenanceAction.Invoke(employees);
            }

            _ = Task.Run(async () =>
            {
                await Task.Delay(TimeSpan.FromHours(3));
                absence.AbsenceTask();
            });
            Debug.WriteLine("Background Timeservice finished");
        }
        else if(_hasLoggedToday != false)
        {
            _hasLoggedToday = false;
            OnDailyReset?.Invoke();
            Debug.WriteLine("Background Timeservice Reset");
        }
    }
}


//private void LogSuccess(string today)
//{
//    try
//    {
//        string logEntry = $"{today} Success";
//        File.AppendAllText(_logFilePath, logEntry + Environment.NewLine);
//        Console.WriteLine($"[LOGGED] {logEntry}");
//    }
//    catch (Exception ex)
//    {
//        Console.WriteLine($"[ERROR] Logging failed: {ex.Message}");
//    }
//}

