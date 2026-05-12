using Microsoft.Extensions.DependencyInjection;
using RestaurantOrderingApp.Layers.BusinessLogicLayer;
using RestaurantOrderingApp.Utils;
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

        public void CloseOrderDetailsWindow()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window is OrderDetailsWindow)
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

        public void ShowOrderDetailsWindow(OrderDisplay orderDisplay)
        {
            var vm = new OrderDetailsVM(_serviceProvider.GetRequiredService<IDialogService>(), orderDisplay);
            var window = new OrderDetailsWindow(vm);
            window.ShowDialog();
        }

        public void ShowOrderHistoryView()
        {
            var restaurantVM = _serviceProvider.GetRequiredService<RestaurantVM>();
            var orderHistoryVM = new OrderHistoryVM(_serviceProvider.GetRequiredService<CurrentUserSession>(),
                _serviceProvider.GetRequiredService<OrderBLL>(), _serviceProvider.GetRequiredService<IDialogService>());
            orderHistoryVM.LoadOrders();
            restaurantVM.CurrentView = orderHistoryVM;
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
