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

        private void BtnAddAllTest(object sender, RoutedEventArgs e)
        {
            vm.AddTest();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedEmployee = (sender as ListBox)?.SelectedItem as Employee;

            if (selectedEmployee != null)
            {
                vm.CheckIn(selectedEmployee);
            }
        }
    }
}
