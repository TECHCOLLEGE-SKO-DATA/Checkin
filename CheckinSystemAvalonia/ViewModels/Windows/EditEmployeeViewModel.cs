using Avalonia.Controls;
using CheckInSystemAvalonia.Platform;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CheckinLibrary.Models;
using CheckInSystemAvalonia.CardReader;
using ReactiveUI;

namespace CheckInSystemAvalonia.ViewModels.Windows
{
    public class EditEmployeeViewModel : ViewModelBase 
    {
        public Employee EditEmployee { get; set; }

        public bool WaitingForCard
        {
            get => State.UpdateCardId;
        }

        public TextBlock? UpdateCardMessage { get; set; }

        public EditEmployeeViewModel(IPlatform platform, Employee editEmployee) : base(platform)
        {
            EditEmployee = editEmployee;
            if (EditEmployee != null)
            {
                EditEmployee.PropertyChanged += UpdateWaitingForCard;
            }
        }

        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            this.PropertyChanged -= UpdateWaitingForCard;
            CardReader.State.ClearUpdateCard();
            EditEmployee.UpdateDb();
        }

        public void UpdateCardId()
        {
            CardReader.State.SetUpdateCard(EditEmployee);
            this.RaisePropertyChanged("WaitingForCard");
            //BindingOperations.GetBindingExpression(UpdateCardMessage, TextBox.TextProperty).UpdateSource();
        }

        private void UpdateWaitingForCard(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "CardID":
                    this.RaisePropertyChanged("WaitingForCard");
                    break;
            }
        }
    }
}
