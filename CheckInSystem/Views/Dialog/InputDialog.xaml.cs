using System.Windows;

namespace CheckInSystem.Views.Dialog;

public partial class InputDialog : Window
{
    public InputDialog(string question, string defaultAnswer = "")
    {
        InitializeComponent();
        lblQuestion.Content = question;
        txtAnswer.Text = defaultAnswer;
    }

    private void Window_ContentRendered(object? sender, EventArgs e)
    {
        txtAnswer.SelectAll();
        txtAnswer.Focus();
    }

    private void btnDialogOk_Click(object sender, RoutedEventArgs e)
    {
        this.DialogResult = true;
    }
    public string Answer
    {
        get { return txtAnswer.Text; }
    }
}