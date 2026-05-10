using RestaurantOrderingApp.ViewModels;
using System.Windows;

namespace RestaurantOrderingApp.Views
{
    public partial class OrderConfirmationWindow : Window
    {
        public OrderConfirmationWindow(OrderConfirmationVM orderConfirmationVM)
        {
            InitializeComponent();
            DataContext = orderConfirmationVM;
        }
    }
}
