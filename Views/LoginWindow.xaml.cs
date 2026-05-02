using RestaurantOrderingApp.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace RestaurantOrderingApp.Views
{
    public partial class LoginWindow : Window
    {
        public LoginWindow(LoginVM loginVM)
        {
            InitializeComponent();
            DataContext = loginVM;
        }
        private void PasswordBox_PasswordChanged(object sender, EventArgs e)
        {
            if (DataContext is LoginVM vm)
            {
                vm.Password = ((PasswordBox)sender).Password;
            }
        }
    }
}
