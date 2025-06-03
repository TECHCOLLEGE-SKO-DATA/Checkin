using Avalonia.Controls;
using CheckInSystemAvalonia.ViewModels.Windows;
using CheckInSystemAvalonia.Views;
using System.Threading.Tasks;

namespace CheckInSystemAvalonia.Controls
{
    public static class MessageBox
    {
        public static async Task<MessageBoxResult> Show(Window owner, string message, string title, MessageBoxButton buttons)
        {
            var window = new MessageBoxWindow();
            var vm = new MessageBoxViewModel(window, message, title, buttons);
            window.DataContext = vm;

            await window.ShowDialog(owner);
            return vm.Result;
        }
    }
}
