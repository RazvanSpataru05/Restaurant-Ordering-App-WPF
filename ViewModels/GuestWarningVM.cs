using RestaurantOrderingApp.Dialog_Service;
using RestaurantOrderingApp.Utils;

namespace RestaurantOrderingApp.ViewModels
{
    public class GuestWarningVM : BaseViewModel
    {
        private readonly IDialogService _dialogService;
        public string InfoMessage { get; set; }
        public RelayCommand LoginCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }
        public GuestWarningVM(IDialogService dialogService, string infoMessage)
        {
            _dialogService = dialogService;
            InfoMessage = infoMessage;

            InitializeCommands();
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
        private void InitializeCommands()
        {
            LoginCommand = new(_ => Login());
            CancelCommand = new(_ => Cancel());
        }
    }
}
