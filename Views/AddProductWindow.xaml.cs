using RestaurantOrderingApp.ViewModels;
using System.Windows;

namespace RestaurantOrderingApp.Views
{
    public partial class AddProductWindow : Window
    {
        public AddProductWindow(AddProductVM vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
