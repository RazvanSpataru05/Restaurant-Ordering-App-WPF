using RestaurantOrderingApp.Dialog_Service;
using RestaurantOrderingApp.Utils;

namespace RestaurantOrderingApp.ViewModels
{
    public class GuestWarningVM : BaseViewModel
    {
        private readonly IDialogService _dialogService;
        public string InfoMessage { get; } = "You must be logged in to add products to cart!";
        public RelayCommand LoginCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }
        public GuestWarningVM(IDialogService dialogService)
        {
            _dialogService = dialogService;
            LoginCommand = new(_ => Login());
            CancelCommand = new(_ => Cancel());
        }
        private void Login()
        {
            _dialogService.ShowLoginWindow();
            _dialogService.CloseGuestWarningWindow();
            _dialogService.CloseRestaurantWindow();
        }
        private void Cancel()
        {
            _dialogService.CloseGuestWarningWindow();
        }
    }
}
