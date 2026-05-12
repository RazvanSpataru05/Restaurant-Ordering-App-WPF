using RestaurantOrderingApp.ViewModels;
using System.Windows;

namespace RestaurantOrderingApp.Views
{
    public partial class OrderDetailsWindow : Window
    {
        public OrderDetailsWindow(OrderDetailsVM vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
