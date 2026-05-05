using RestaurantOrderingApp.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace RestaurantOrderingApp.Views
{
    public partial class RestaurantWindow : Window
    {
        public RestaurantWindow(RestaurantVM restaurantVM)
        {
            InitializeComponent();
            DataContext = restaurantVM;
        }
        private void CloseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }
    }
     
}
