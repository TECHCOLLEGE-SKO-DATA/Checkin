using System.ComponentModel;
using System.Runtime.CompilerServices;
using Dapper;
using System.Data.SqlClient;
using CheckInSystem.Database;

namespace CheckInSystem.Models;


public class Employee : INotifyPropertyChanged
{
    DatabaseHelper databaseHelper = new();

    public int ID { get; private set; }
    private string _cardID;
    public string CardID
    {
        get => _cardID;
        set => SetProperty(ref _cardID, value);
    }
    
    private string? _firstName;
    public string? FirstName
    {
        get => _firstName;
        set => SetProperty(ref _firstName, value);
    }

    private string? _middleName;
    public string? MiddleName
    {
        get => _middleName;
        set => SetProperty(ref _middleName, value);
    }
    public string? MiddleNameShort
    {
        get { return ShortenName(_middleName); }
    }
    private string? _lastName;

    public string? LastName
    {
        get => _lastName;
        set => SetProperty(ref _lastName, value);
    }
    
    private bool _isOffSite;
    public bool IsOffSite
    {
        get => _isOffSite;
        set => SetProperty(ref _isOffSite, value);
    }
    
    public DateTime? OffSiteUntil { get; set; }

    private bool _isCheckedIn;
    public bool IsCheckedIn
    {
        get => _isCheckedIn;
        set => SetProperty(ref _isCheckedIn, value);
    }

    private DateTime? _arrivalTime;
    public DateTime? ArrivalTime
    {
        get => _arrivalTime;
        set => SetProperty(ref _arrivalTime, value);
    }

    private DateTime? _departureTime;
    public DateTime? DepartureTime
    {
        get => _departureTime;
        set => SetProperty(ref _departureTime, value);
    }
    
    public void CardScanned(string cardID)
    {
        Employee? tempEmployee = DatabaseHelper.GetFromCardId(cardID);
        if (tempEmployee == null) return;

        SetProperty(ref _arrivalTime, tempEmployee.ArrivalTime, nameof(ArrivalTime));
        SetProperty(ref _departureTime, tempEmployee.DepartureTime, nameof(DepartureTime));
        SetProperty(ref _isCheckedIn, tempEmployee.IsCheckedIn, nameof(IsCheckedIn));
    }

    public void GetUpdatedSiteTimes()
    {
        var siteTimes = databaseHelper.GetUpdatedSiteTimes(ID);

        ArrivalTime = siteTimes.ArrivalTime;
        DepartureTime = siteTimes.DepartureTime;
    }

    public void UpdateDb()
    {
        databaseHelper.UpdateDb(CardID, FirstName, MiddleName, LastName, IsOffSite, OffSiteUntil, ID);
    }

    public void DeleteFromDb()
    {
        databaseHelper.DeleteFromDb(this.ID);
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
   public string ShortenName(string middelname)
    {
        if (middelname == null)
        {
            return "";
        }
        string[] names = middelname.Split(" ");
        string shortenName = "";
        string seperate = ". ";
        foreach (string name in names)
        {
            string letter = name.Substring(0, 1);
            shortenName += letter + seperate;
        }
        shortenName = shortenName.Trim();
        return shortenName;
    }
    public Employee(int id, string firstName, bool isCheckedIn)
    {
        ID = id;
        FirstName = firstName;
        IsCheckedIn = isCheckedIn;
    }
    public Employee() { }
}

