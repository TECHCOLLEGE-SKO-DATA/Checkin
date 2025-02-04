using System.Windows;

namespace CheckInSystem.Views.Dialog;

public partial class LoadingStartup : Window
{
    public static LoadingStartup? Instance { get; private set; }
    public LoadingStartup()
    {
        InitializeComponent();
        if (Instance == null)
        {
            Instance = this;
        }
    }
}