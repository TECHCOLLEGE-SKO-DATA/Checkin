using Avalonia.Controls;
using CheckinLibrary.Database;
using CheckinLibrary.Models;
using CheckInSystemAvalonia.CardReader;
using CheckInSystemAvalonia.Platform;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Text;

namespace CheckInSystemAvalonia.ViewModels.UserControls
{
    public class FakeNFCViewModel : ViewModelBase
    {
        private readonly DatabaseHelper dbHelper = new();
        private readonly Random random = new();

        private string _newCardId;
        public string NewCardId
        {
            get => _newCardId;
            set => this.RaiseAndSetIfChanged(ref _newCardId, value);
        }

        private Employee _selectedEmployee;
        public Employee SelectedEmployee
        {
            get => _selectedEmployee;
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedEmployee, value);
                if (value != null)
                    CheckIn(value);
            }
        }

        public ObservableCollection<Employee> Employees { get; } = new();

        public ReactiveCommand<Unit, Unit> ScanNewCardCommand { get; }
        public ReactiveCommand<Unit, Unit> GetFromDatabaseCommand { get; }

        private ScriptedCardReader _cardReader => (ScriptedCardReader)_platform.CardReader;

        public FakeNFCViewModel(IPlatform platform) : base(platform)
        {
            NewCardId = RandomCardGen();

            if (!Design.IsDesignMode)
            {
                foreach (var emp in dbHelper.GetAllEmployees())
                Employees.Add(emp);
            }

            ScanNewCardCommand = ReactiveCommand.Create(ScanNewCard);
            GetFromDatabaseCommand = ReactiveCommand.Create(RefreshEmployees);
        }

        private void ScanNewCard()
        {
            if (NewCardId.Length == 11)
            {
                //Add the Actual method for scaning new car
                _cardReader.TriggerCardInserted(NewCardId);
            }
            else
            {

                _cardReader.TriggerCardInserted(RandomCardGen());
            }
            Employees.Clear();
            foreach (var employee in dbHelper.GetAllEmployees())
            {
                Employees.Add(employee);
            }
        }

        private void CheckIn(Employee employee)
        {
            _cardReader.TriggerCardInserted(employee.CardID);
        }

        private void RefreshEmployees()
        {
            Employees.Clear();
            foreach (var emp in dbHelper.GetAllEmployees())
                Employees.Add(emp);
        }

        private string RandomCardGen()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            StringBuilder result = new();

            for (int i = 0; i < 11; i++)
                result.Append(chars[random.Next(chars.Length)]);

            return result.ToString();
        }
    }
}
