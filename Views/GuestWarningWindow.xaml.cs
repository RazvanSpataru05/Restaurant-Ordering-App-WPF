using RestaurantOrderingApp.ViewModels;
using System.Windows;

namespace RestaurantOrderingApp.Views
{
    public partial class GuestWarningWindow : Window
    {
        public GuestWarningWindow(GuestWarningVM guestWarningVM)
        {
            InitializeComponent();
            DataContext = guestWarningVM;
        }
    }
}
