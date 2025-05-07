using CheckinSystemAvalonia.Platform;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace CheckinSystemAvalonia.ViewModels.Windows
{
    public class MessageBoxViewModel : ViewModelBase
    {
        public string Title {  get; set; }

        public string ErrorMessage { get; set; }

        public MessageBoxViewModel(IPlatform platform, string title, string errorMessage) : base(platform)
        {
            Title = title;

            ErrorMessage = errorMessage;
        }
    }
}
