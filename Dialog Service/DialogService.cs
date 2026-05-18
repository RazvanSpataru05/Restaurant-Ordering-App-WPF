using Microsoft.Extensions.DependencyInjection;
using RestaurantOrderingApp.Display;
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

        public void CloseAddProductWindow()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window is AddProductWindow)
                {
                    window.Close();
                    return;
                }
            }
        }

        public void CloseGuestWarningWindow()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window is GuestWarningWindow)
                {
                    window.Close();
                    return;
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

        public void CloseLogoutWindow()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window is LogoutWindow)
                {
                    window.Close();
                    return;
                }
            }
        }

        public void CloseManageMenuWindow()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window is ManageMenuWindow)
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
                    return;
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
                    return;
                }
            }
        }

        public void CloseProductDetailWindow()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window is ProductDetailWindow)
                {
                    window.Close();
                    return;
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
                    return;
                }
            }
        }

        public object? GetService(Type serviceType)
        {
            throw new NotImplementedException();
        }

        public void ShowAddProductWindow()
        {
            var vm = new AddProductVM(_serviceProvider.GetRequiredService<IDialogService>(),
                _serviceProvider.GetRequiredService<ProductBLL>(), _serviceProvider.GetRequiredService<CategoryBLL>());
            var window = new AddProductWindow(vm);
            window.ShowDialog();
        }

        public void ShowAdminView()
        {
            var restaurantVM = _serviceProvider.GetRequiredService<RestaurantVM>();
            var adminPanelVM = _serviceProvider.GetRequiredService<AdminPanelVM>();
            adminPanelVM.LoadOrders();
            restaurantVM.CurrentView = adminPanelVM;
        }

        public void ShowGuestWarningWindow(string infoMessage)
        {
            var vm = new GuestWarningVM(_serviceProvider.GetRequiredService<IDialogService>(), infoMessage);
            var window = new GuestWarningWindow(vm);
            window.ShowDialog();
        }

        public void ShowLoginWindow()
        {
            var vm = _serviceProvider.GetRequiredService<LoginVM>();
            var window = new LoginWindow(vm);
            window.Show();
        }

        public void ShowLogoutWindow()
        {
            var cartService = _serviceProvider.GetRequiredService<CartService>();
            var vm = new LogoutVM(_serviceProvider.GetRequiredService<IDialogService>());
            vm.InfoMessage = cartService.Items.Count > 0 ?
                "You have items in your cart. Are you sure you want to log out? Your cart will be cleared." :
                "Are you sure you want to log out?";
            var window = new LogoutWindow(vm);
            window.ShowDialog();
        }

        public void ShowManageMenuWindow()
        {
            var vm = new ManageMenuVM(_serviceProvider.GetRequiredService<IDialogService>(),
                _serviceProvider.GetRequiredService<MenuBLL>(),
                _serviceProvider.GetRequiredService<ProductBLL>());
            var window = new ManageMenuWindow(vm);
            window.ShowDialog();
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

        public void ShowProductDetailWindow(ProductDisplay? productDisplay)
        {
            if (productDisplay == null) return;
            var vm = new ProductDetailVM(_serviceProvider.GetRequiredService<IDialogService>(), productDisplay);
            var window = new ProductDetailWindow(vm);
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
