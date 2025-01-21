using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using CheckInSystem.Models;

namespace CheckInSystem.ViewModels;

public class ViewmodelBase : INotifyPropertyChanged
{
    public static ObservableCollection<Employee> Employees { get; set; }
    public static ObservableCollection<Group> Groups { get; set; }
    
    public static ContentControl MainContentControl { get; set; }
    
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