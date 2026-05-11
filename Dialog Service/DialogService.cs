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
        public void CloseGuestWarningWindow()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window is GuestWarningWindow)
                {
                    window.Close();
                    break;
                }
            }
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

        public void CloseOrderConfirmationWindow()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window is OrderConfirmationWindow)
                {
                    window.Close();
                    break;
                }
            }
        }

        public void CloseRestaurantWindow()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window is RestaurantWindow)
                {
                    window.Close();
                    break;
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

        public void ShowGuestWarningWindow(string infoMessage)
        {
            var vm = new GuestWarningVM(_serviceProvider.GetRequiredService<IDialogService>(), infoMessage);
            var window = new GuestWarningWindow(vm);
            window.ShowDialog();
        }

        public void ShowLoginWindow()
        {
            var loginVM = _serviceProvider.GetRequiredService<LoginVM>();
            var window = new LoginWindow(loginVM);
            window.Show();
        }

        public void ShowOrderConfirmationWindow(string orderCode, string estimatedDeliveryTime)
        {
            var vm = new OrderConfirmationVM(_serviceProvider.GetRequiredService<IDialogService>(),
                orderCode, estimatedDeliveryTime);
            var window = new OrderConfirmationWindow(vm);
            window.ShowDialog();
        }

        public void ShowRestaurantWindow()
        {
            var restaurantVM = _serviceProvider.GetRequiredService<RestaurantVM>();
            var window = new RestaurantWindow(restaurantVM);
            window.Show();
        }

        public void ShowWelcomeView()
        {
            var restaurantVM = _serviceProvider.GetService<RestaurantVM>();
            restaurantVM.CurrentView = null;
        }
    }
}
