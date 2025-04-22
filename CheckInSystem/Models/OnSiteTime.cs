using System.ComponentModel;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using CheckInSystem.Database;
using Dapper;

namespace CheckInSystem.Models;

public class OnSiteTime : INotifyPropertyChanged
{
    DatabaseHelper databaseHelper;

    private int _id;
    public int Id
    {
        get => _id;
        private set => SetProperty(ref _id, value);
    }

    private int _employeeID;
    public int EmployeeID
    {
        get => _employeeID;
        set => SetProperty(ref _employeeID, value);
    }

    private DateTime? _oldArrivalTime;
    private DateTime? _arrivalTime;
    public DateTime? ArrivalTime
    {
        get => _arrivalTime;
        set => SetProperty(ref _arrivalTime, value);
    }

    private DateTime? _oldDepartureTime;
    private DateTime? _departureTime;
    public DateTime? DepartureTime
    {
        get => _departureTime;
        set => SetProperty(ref _departureTime, value);
    }

    public OnSiteTime()
    {
        
    }
    public OnSiteTime(int id, int employeeId, DateTime arrivalTime, DateTime? departureTime)
    {
        _id = id;
        EmployeeID = employeeId;
        ArrivalTime = arrivalTime;
        DepartureTime = departureTime;
        _oldArrivalTime = arrivalTime;
        _oldDepartureTime = departureTime;
    } 
    public OnSiteTime(OnSiteTime newOnSiteTime)
    {
        _id = newOnSiteTime.Id;
        EmployeeID = newOnSiteTime.EmployeeID;
        ArrivalTime = newOnSiteTime.ArrivalTime;
        DepartureTime = newOnSiteTime.DepartureTime;
        _oldArrivalTime = newOnSiteTime.ArrivalTime;
        _oldDepartureTime = newOnSiteTime.DepartureTime;
    }

    public static List<OnSiteTime> GetOnsiteTimesForEmployee(Employee employee)
    {     
        DatabaseHelper databaseHelper = new ();
        return databaseHelper.GetOnsiteTimesForEmployee(employee);
    }

    public bool IsChanged()
    {
        if (_oldArrivalTime != ArrivalTime) return true;
        if (_oldDepartureTime != DepartureTime) return true;
        return false;
    }

    public void RevertTopreviousTime()
    {
        ArrivalTime = _oldArrivalTime;
        DepartureTime = _oldDepartureTime;
    }

    public void DeleteFromDb()
    {
        databaseHelper.DeleteFromDbOnSiteTime(Id);
    }

    public static void UpdateMutipleSiteTimes(List<OnSiteTime> siteTimes)
    {
        DatabaseHelper databaseHelper = new();
        databaseHelper.UpdateMutipleSiteTimes(siteTimes);
    }

    public static OnSiteTime AddTimeToDb(int employeeId, DateTime arrivalTime, DateTime? departureTime)
    {
        DatabaseHelper databaseHelper = new();
        return databaseHelper.AddTimeToDb(employeeId, arrivalTime, departureTime);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        if (PropertyChanged != null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    protected void SetProperty<T>(ref T variable, T value, [CallerMemberName] string? propertyName = null)
    {
        if (!EqualityComparer<T>.Default.Equals(variable, value))
        {
            variable = value;
            OnPropertyChanged(propertyName);
        }
    }
}
