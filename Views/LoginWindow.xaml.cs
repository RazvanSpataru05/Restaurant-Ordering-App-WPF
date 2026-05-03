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
            loginVM.RegisterVM.OnClearPasswords += () =>
            {
                RegisterPasswordBox.Clear();
                RegisterConfirmPasswordBox.Clear();
            };
        }
        private void PasswordBox_PasswordChanged(object sender, EventArgs e)
        {
            if (DataContext is LoginVM vm)
            {
                vm.Password = ((PasswordBox)sender).Password;
            }
        }
        private void RegisterPasswordBox_PasswordChanged(object sender, EventArgs e)
        {
            if (DataContext is LoginVM vm)
            {
                vm.RegisterVM.Password = ((PasswordBox)sender).Password;
            }
        }
        private void RegisterConfirmPasswordBox_PasswordChanged(object sender, EventArgs e)
        {
            if (DataContext is LoginVM vm)
            {
                vm.RegisterVM.ConfirmPassword = ((PasswordBox)sender).Password;
            }
        }
    }
}
