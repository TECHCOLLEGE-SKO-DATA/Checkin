using System.Text;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CheckInSystem.Models;
using CheckInSystem.ViewModels.UserControls;

namespace CheckInSystem.Views.UserControls
{
    public partial class FakeNFCView : UserControl
    {
        
        public FakeNFCViewModel _vm  { get => (FakeNFCViewModel) DataContext; set => DataContext = value; }
        public FakeNFCViewModel FakeNFCViewModel  { get => _vm; set => _vm = value; }
        public FakeNFCView()
        {
            InitializeComponent();
        }
        public void BtnScannewCard(object sender, RoutedEventArgs e)
        {
            _vm.ScanNewCard();
        }

        public void BtnGetFromDatabase(object sender, RoutedEventArgs e)
        {
            _vm.GetDataFromDB();
        }
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox.Text.Length >= 11)
            {
                e.Handled = true;
            }
        }

        private async void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listBox = sender as ListBox;
            var selectedEmployee = listBox?.SelectedItem as Employee;

            if (selectedEmployee != null)
            {
                selectedEmployee.FontBoldNormal = FontWeights.Bold;

                listBox.ItemsSource = null;
                listBox.ItemsSource = _vm.Employees;

                _vm.CheckIn(selectedEmployee);

                //3 sec befor turning it back to normal font
                await Task.Delay(3000);

                selectedEmployee.FontBoldNormal = FontWeights.Normal;

                listBox.ItemsSource = null;
                listBox.ItemsSource = _vm.Employees;
            }
        }        
    }  
}
