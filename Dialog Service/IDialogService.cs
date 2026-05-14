using RestaurantOrderingApp.Utils;

namespace RestaurantOrderingApp.Dialog_Service
{
    public interface IDialogService
    {
        void ShowLoginWindow();
        void ShowRestaurantWindow();
        void CloseLoginWindow();
        void CloseRestaurantWindow(); 
        void ShowGuestWarningWindow(string infoMessage);
        void CloseGuestWarningWindow();
        void ShowWelcomeView();
        void ShowAdminView();
        void ShowOrderHistoryView();
        void ShowOrderConfirmationWindow(string orderCode, string estimatedDeliveryTime);
        void CloseOrderConfirmationWindow();
        void ShowOrderDetailsWindow(OrderDisplay orderDisplay);
        void CloseOrderDetailsWindow();
        void ShowAddProductWindow();
        void CloseAddProductWindow();
        void ShowManageMenuWindow();
        void CloseManageMenuWindow();
    }
}
