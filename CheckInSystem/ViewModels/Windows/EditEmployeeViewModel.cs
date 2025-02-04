﻿using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Data;
using CheckInSystem.CardReader;
using CheckInSystem.Models;
using CheckInSystem.Platform;

namespace CheckInSystem.ViewModels.Windows;

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
        EditEmployee.PropertyChanged += UpdateWaitingForCard;
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
        OnPropertyChanged("WaitingForCard");
        //BindingOperations.GetBindingExpression(UpdateCardMessage, TextBox.TextProperty).UpdateSource();
    }

    private void UpdateWaitingForCard(object sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case "CardID":
                OnPropertyChanged("WaitingForCard");
                break;
        }
    }
}