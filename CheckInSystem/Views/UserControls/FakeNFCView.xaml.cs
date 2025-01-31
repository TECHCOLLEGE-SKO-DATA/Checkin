using System.Windows;
using System.Windows.Controls;
using CheckInSystem.Models;
using CheckInSystem.ViewModels.UserControls;

namespace CheckInSystem.Views.UserControls
{
    public partial class FakeNFCView : UserControl
    {
        private FakeNFCViewModel vm;

        public FakeNFCView()
        {
            InitializeComponent();
            vm = new FakeNFCViewModel();
            DataContext = vm;
        }

        public void BtnGetFromDatabase(object sender, RoutedEventArgs e)
        {
            vm.GetDataFromDB();
        }
        
        private void BtnAddAllTest(object sender, RoutedEventArgs e)
        {
            vm.AddTest();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listBox = sender as ListBox;
            var selectedEmployee = listBox?.SelectedItem as Employee;

            if (selectedEmployee != null)
            {
                if (selectedEmployee.FontBoldNormal == FontWeights.Bold)
                {
                    selectedEmployee.FontBoldNormal = FontWeights.Normal;
                }
                else 
                {
                    selectedEmployee.FontBoldNormal = FontWeights.Bold;
                }

                listBox.ItemsSource = null;
                listBox.ItemsSource = vm.Employees;

                vm.CheckIn(selectedEmployee);
            }
        }

    }
}
