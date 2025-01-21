using System.Windows;

namespace CheckInSystem.Views.Dialog;

public partial class WaitingForCardDialog : Window
{
    public static WaitingForCardDialog? Instance;
    public WaitingForCardDialog()
    {
        InitializeComponent();
        Topmost = true;
    }

    private void Cancel(object sender, RoutedEventArgs e)
    {
        CardReader.State.UpdateNextEmployee = false;
        Close();
    }

    public static void Open()
    {
        //if (Instance == null) Instance = new WaitingForCardDialog();
        Instance = new WaitingForCardDialog();
        Instance.Show();
    }
}