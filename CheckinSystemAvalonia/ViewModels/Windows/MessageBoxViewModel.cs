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

        public MessageBoxViewModel(string title, string errorMessage)
        {
            Title = title;

            ErrorMessage = errorMessage;
        }
    }
}
