using System.Windows;
using System.Windows.Controls;
using CheckInSystem.Database;
using CheckInSystem.Models;
using CheckInSystem.ViewModels.UserControls;

namespace CheckInSystem.Views.UserControls
{
    public partial class FakeNFCView : UserControl
    {
        private FakeNFCViewModel vm;
        DatabaseHelper databaseHelper = new();

        public FakeNFCView()
        {
            InitializeComponent();
            DataContext = new FakeNFCViewModel();
        }

        private void BtnAddEmployee(object sender, RoutedEventArgs e)
        {
            
        }

        private void BtnRemoveEmployee(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
