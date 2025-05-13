using CheckInSystemAvalonia.Platform;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace CheckInSystemAvalonia.ViewModels.Windows
{
    public class MessageBoxViewModel : ViewModelBase
    {
        public string Title {  get; set; }

        public string ErrorMessage { get; set; }

        public ReactiveCommand<Unit,Unit> Btn_Ok { get; }

        public MessageBoxViewModel(IPlatform platform, string title, string errorMessage) : base(platform)
        {
            Title = title;

            ErrorMessage = errorMessage;
        }
    }
}
