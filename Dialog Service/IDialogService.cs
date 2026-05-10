namespace RestaurantOrderingApp.Dialog_Service
{
    public interface IDialogService
    {
        void ShowLoginWindow();
        void ShowRestaurantWindow();
        void CloseLoginWindow();
        void CloseRestaurantWindow(); 
        bool ShowConfirmationDialog(string message);
        void ShowGuestWarningWindow();
        void CloseGuestWarningWindow();
        void ShowWelcomeView();
        void ShowOrderConfirmationWindow(string orderCode, string estimatedDeliveryTime);
        void CloseOrderConfirmationWindow();
    }
}
