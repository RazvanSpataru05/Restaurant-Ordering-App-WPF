using Microsoft.Extensions.DependencyInjection;
using RestaurantOrderingApp.ViewModels;
using RestaurantOrderingApp.Views;
using System.Windows;

namespace RestaurantOrderingApp.Dialog_Service
{
    public class DialogService : IDialogService, IServiceProvider
    {
        private readonly IServiceProvider _serviceProvider;

        public DialogService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void CloseLoginWindow()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window is LoginWindow)
                {
                    window.Close();
                    return;
                }
            }
        }

        public object? GetService(Type serviceType)
        {
            throw new NotImplementedException();
        }

        public bool ShowConfirmationDialog(string message)
        {
            return MessageBox.Show(message, "Notice",
                MessageBoxButton.OK,
                MessageBoxImage.Information) == MessageBoxResult.OK;
        }

        public void ShowLoginWindow()
        {
            var loginVM = _serviceProvider.GetRequiredService<LoginVM>();
            var window = new LoginWindow(loginVM);
            window.Show();
        }

        public void ShowRestaurantWindow()
        {
            var restaurantVM = _serviceProvider.GetRequiredService<RestaurantVM>();
            var window = new RestaurantWindow(restaurantVM);
            window.Show();
        }
    }
}
