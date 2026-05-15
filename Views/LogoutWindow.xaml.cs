using RestaurantOrderingApp.ViewModels;
using System.Windows;

namespace RestaurantOrderingApp.Views
{
    public partial class LogoutWindow : Window
    {
        public LogoutWindow(LogoutVM vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
