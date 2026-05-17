using RestaurantOrderingApp.ViewModels;
using System.Windows;

namespace RestaurantOrderingApp.Views
{
    public partial class RestaurantWindow : Window
    {
        public RestaurantWindow(RestaurantVM restaurantVM)
        {
            InitializeComponent();
            DataContext = restaurantVM;
        }
    }
     
}
