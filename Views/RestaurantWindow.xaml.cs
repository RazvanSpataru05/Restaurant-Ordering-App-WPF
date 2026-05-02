using RestaurantOrderingApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

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
