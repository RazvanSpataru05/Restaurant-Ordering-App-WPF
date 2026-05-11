namespace RestaurantOrderingApp.Dialog_Service
{
    public interface IDialogService
    {
        void ShowLoginWindow();
        void ShowRestaurantWindow();
        void CloseLoginWindow();
        void CloseRestaurantWindow(); 
        bool ShowConfirmationDialog(string message);
        void ShowGuestWarningWindow(string infoMessage);
        void CloseGuestWarningWindow();
        void ShowWelcomeView();
        void ShowOrderConfirmationWindow(string orderCode, string estimatedDeliveryTime);
        void CloseOrderConfirmationWindow();
    }
}
