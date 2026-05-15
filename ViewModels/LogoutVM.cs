using RestaurantOrderingApp.Dialog_Service;
using RestaurantOrderingApp.Utils;

namespace RestaurantOrderingApp.ViewModels
{
    public class LogoutVM : BaseViewModel
    {
        private readonly IDialogService _dialogService;

        public string InfoMessage { get; set; }
        public RelayCommand YesCommand { get; set; }
        public RelayCommand NoCommand { get; set; }

        public LogoutVM(IDialogService dialogService)
        {
            _dialogService = dialogService;

            InitializeCommands();
        }
        private void InitializeCommands()
        {
            YesCommand = new(_ =>
            {
                _dialogService.ShowLoginWindow();
                _dialogService.CloseRestaurantWindow();
                _dialogService.CloseLogoutWindow();
            });
            NoCommand = new(_ => _dialogService.CloseLogoutWindow());
        }
    }
}
