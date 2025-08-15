using CheckinLibrary.Background_tasks;
using CheckinLibrary.Database;
using CheckinLibrary.Models;
using System.Collections.ObjectModel;
public class BackgroundTimeService
{
    private readonly DatabaseHelper _dbHelper;
    AbsencBackGroundService absence = new();

    private readonly TimeSpan _checkInterval = TimeSpan.FromMinutes(60);
    private readonly TimeSpan _startTime = new TimeSpan(21, 0, 0);  // 21:00 (9 PM)
    private readonly TimeSpan _endTime = new TimeSpan(1, 0, 0);    // 01:00 (1 AM)

    private bool _hasLoggedToday = false;
    private CancellationTokenSource _cts;
    private readonly Func<DateTime> _timeProvider;
    
    public event Action OnDailyReset;
    public Action<List<Employee>> PerformMaintenanceAction { get; set; } = _ => { };

    public Func<List<Employee>> GetEmployees { get; set; }


    public BackgroundTimeService(Func<DateTime> timeProvider = null, DatabaseHelper dbHelper = null)
    {
        _timeProvider = timeProvider ?? (() => DateTime.Now);
    }

    public void Start()
    {
        this.PerformMaintenanceAction = (employees) =>
        {
            Maintenance.CheckOutEmployeesIfTheyForgot(employees);
            Maintenance.CheckForEndedOffSiteTime(employees);
        };

        _cts = new CancellationTokenSource();
        Task.Run(async () =>
        {
            while (!_cts.Token.IsCancellationRequested)
            {
                CheckTime();
                await Task.Delay(_checkInterval);
            }
        });
    }

    public void Stop()
    {
        _cts?.Cancel();
    }

    public async void CheckTime()
    {
        var currentTime = _timeProvider().TimeOfDay;

        if ((currentTime >= _startTime || currentTime < _endTime) && !_hasLoggedToday)
        {
            // Retrieve employees: use the test delegate if set, otherwise query the database
            List<Employee> employees;

            if (GetEmployees != null)
            {
                employees = GetEmployees(); // Use fake/test employees
            }
            else
            {
                employees = _dbHelper.GetAllEmployees(); // Use real database
            }


            _hasLoggedToday = true;
            PerformMaintenanceAction.Invoke(employees);

            // Run absence check 8 hours later
            _ = Task.Run(async () =>
            {
                await Task.Delay(TimeSpan.FromHours(8));
                absence.AbsenceTask();
            });
        }
        else if (_timeProvider().Hour == 5 || _timeProvider().Hour == 6)
        {
            _hasLoggedToday = false;
            OnDailyReset?.Invoke();
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

