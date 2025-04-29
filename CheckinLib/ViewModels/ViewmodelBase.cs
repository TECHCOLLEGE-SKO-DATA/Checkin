using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using CheckinLib.Models;
using CheckinLib.Platform;

namespace CheckinLib.ViewModels;

public class ViewModelBase : INotifyPropertyChanged
{
    //public ObservableCollection<Employee> Employees => _platform.MainWindowViewModel.Employees;
    //public static ObservableCollection<Group> Groups { get; set; }
    
    //public static ContentControl MainContentControl { get; set; }
    
    public event PropertyChangedEventHandler? PropertyChanged;
    protected IPlatform _platform;
    public ViewModelBase(IPlatform platform)
    {
        _platform = platform;
    }
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