using CheckinLibrary.Background_tasks;
public class BackgroundTimeService
{
    AbsencBackGroundService absence = new();

    private readonly TimeSpan _checkInterval = TimeSpan.FromMinutes(60);
    private readonly TimeSpan _startTime = new TimeSpan(21, 0, 0);  // 21:00 (9 PM)
    private readonly TimeSpan _endTime = new TimeSpan(1, 0, 0);    // 01:00 (1 AM)

    private bool _hasLoggedToday = false;
    private CancellationTokenSource _cts;
    private readonly Func<DateTime> _timeProvider;
    
    public event Action OnDailyReset;
    public Action PerformMaintenanceAction { get; set; } = () => { };

    public BackgroundTimeService(Func<DateTime> timeProvider = null)
    {
        _timeProvider = timeProvider ?? (() => DateTime.Now);
    }

    public void Start()
    {
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
            _hasLoggedToday = true;
            PerformMaintenanceAction.Invoke();

            // Run absence check 8 hours later (approx. 01:00–04:00)
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

