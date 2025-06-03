using Avalonia.Controls;
using ReactiveUI;
using System.Reactive;
using CheckInSystemAvalonia.Controls;

namespace CheckInSystemAvalonia.ViewModels.Windows
{
    public class MessageBoxViewModel : ReactiveObject
    {
        public string Title { get; }
        public string Message { get; }
        public MessageBoxButton ButtonType { get; }

        public bool ShowYes => ButtonType == MessageBoxButton.YesNo || ButtonType == MessageBoxButton.YesNoCancel;
        public bool ShowNo => ShowYes;
        public bool ShowOK => ButtonType == MessageBoxButton.OK || ButtonType == MessageBoxButton.OKCancel;
        public bool ShowCancel => ButtonType == MessageBoxButton.OKCancel || ButtonType == MessageBoxButton.YesNoCancel;

        public ReactiveCommand<Unit, Unit> YesCommand { get; }
        public ReactiveCommand<Unit, Unit> NoCommand { get; }
        public ReactiveCommand<Unit, Unit> OKCommand { get; }
        public ReactiveCommand<Unit, Unit> CancelCommand { get; }

        private readonly Window _window;
        public MessageBoxResult Result { get; private set; } = MessageBoxResult.None;

        public MessageBoxViewModel(Window window, string message, string title, MessageBoxButton buttons)
        {
            _window = window;
            Title = title;
            Message = message;
            ButtonType = buttons;

            YesCommand = ReactiveCommand.Create(() => { Result = MessageBoxResult.Yes; _window.Close(); });
            NoCommand = ReactiveCommand.Create(() => { Result = MessageBoxResult.No; _window.Close(); });
            OKCommand = ReactiveCommand.Create(() => { Result = MessageBoxResult.OK; _window.Close(); });
            CancelCommand = ReactiveCommand.Create(() => { Result = MessageBoxResult.Cancel; _window.Close(); });
        }
    }
}
