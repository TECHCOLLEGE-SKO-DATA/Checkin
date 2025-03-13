using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using CheckInSystem.Database;
using Dapper;

namespace CheckInSystem.Models;


public class Group : INotifyPropertyChanged
{
    DatabaseHelper databaseHelper = new();

    public int ID { get; private set; }
    
    private string _name;
    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    private bool _isvisible;
    public bool Isvisible
    {
        get => _isvisible;
        set => SetProperty(ref _isvisible, value);
    }

    public ObservableCollection<Employee> Members { get; private set; } = new();

    public static List<Group> GetAllGroups(List<Employee> employees)
    {
        DatabaseHelper databaseHelper = new();
        return databaseHelper.GetAllGroups(employees);
    }

    public static Group NewGroup(String name)
    {
        DatabaseHelper databaseHelper = new();
        return databaseHelper.NewGroup(name);
    }

    public void RemoveGroupDb()
    {
        databaseHelper.RemoveGroupDb(ID);
    }

    public void UpdateName(string name, int id)
    {
        this.ID = id;
        this.Name = databaseHelper.UpdateName(name, ID);
    }

    public void Updatevisibility(bool visibility)
    {
        databaseHelper.UpdateVisibility(visibility,Isvisible,ID);
    }
    
    public void AddEmployee(Employee employee)
    {
        if (databaseHelper.AddEmployee(employee, Members, ID))
        {
            Members.Add(employee);
        }
    }

    public void RemoveEmployee(Employee employee)
    {
        if(databaseHelper.RemoveEmployee(employee, Members, ID))
        {
            Members.Remove(employee);
        }
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
    public Group(int id, string name)
    {
        ID = id;
        Name = name;
        Members = new ObservableCollection<Employee>();
    }

    public void InitializeMembers(IEnumerable<Employee> employees)
    {
        foreach (var emp in employees)
            Members.Add(emp);
    }
    public Group()
    { }

}
