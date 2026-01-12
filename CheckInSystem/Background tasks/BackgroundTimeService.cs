using CheckinLibrary.Database;
using CheckinLibrary.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public class BackgroundTimeService
{
    private readonly TimeSpan _checkInterval = TimeSpan.FromMinutes(1);
    private readonly TimeSpan _startTime = new(1, 0, 0);
    private readonly TimeSpan _endTime = new(4, 0, 0);

    private bool _hasRunToday = false;
    private CancellationTokenSource _cts;

    public Func<List<Employee>> GetEmployeesSnapshot { get; set; }

    public Action RunDailyResetOnUI { get; set; }

    public void Start()
    {
        _cts = new CancellationTokenSource();

        Task.Run(async () =>
        {
            while (!_cts.IsCancellationRequested)
            {
                CheckTime();
                await Task.Delay(_checkInterval);
            }
        });
    }

    public void Stop() => _cts?.Cancel();

    private void CheckTime()
    {
        var now = DateTime.Now.TimeOfDay;

        if (now >= _startTime && now < _endTime)
        {
            if (_hasRunToday) return;

            _hasRunToday = true;

            var employees = GetEmployeesSnapshot?.Invoke();
            if (employees == null) return;

            Maintenance.CheckOutEmployeesIfTheyForgot(employees);
            Maintenance.CheckForEndedOffSiteTime(employees);
        }
        else
        {
            if (!_hasRunToday) return;

            _hasRunToday = false;

            RunDailyResetOnUI?.Invoke();
        }
    }
}
