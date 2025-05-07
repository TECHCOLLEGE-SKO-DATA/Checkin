using CheckinLib.Database;
using CheckinLib.Models;
using CheckinSystemAvalonia.CardReader;
using CheckinSystemAvalonia.Platform;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Text;

namespace CheckinSystemAvalonia.ViewModels.UserControls
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

            foreach (var emp in dbHelper.GetAllEmployees())
                Employees.Add(emp);

            ScanNewCardCommand = ReactiveCommand.Create(ScanNewCard);
            GetFromDatabaseCommand = ReactiveCommand.Create(RefreshEmployees);
        }

        private void ScanNewCard()
        {
            var cardId = (NewCardId?.Length == 11) ? NewCardId : RandomCardGen();
            _cardReader.TriggerCardInserted(cardId);
            RefreshEmployees();
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
