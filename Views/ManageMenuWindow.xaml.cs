using RestaurantOrderingApp.ViewModels;
using System.Windows;

namespace RestaurantOrderingApp.Views
{
    public partial class ManageMenuWindow : Window
    {
        public ManageMenuWindow(ManageMenuVM vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
