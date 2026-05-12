using RestaurantOrderingApp.Dialog_Service;
using RestaurantOrderingApp.Utils;

namespace RestaurantOrderingApp.ViewModels
{
    public class OrderConfirmationVM : BaseViewModel
    {
        private readonly IDialogService _dialogService;

        public string OrderCode { get; set; }
        public string EstimatedDelivery { get; set; }

        public RelayCommand CloseCommand { get; set; }

        public OrderConfirmationVM(IDialogService dialogService, string orderCode, string estimatedDelivery)
        {
            _dialogService = dialogService;
            OrderCode = "ORD-" + orderCode.Substring(0, 8);
            EstimatedDelivery = estimatedDelivery;

            CloseCommand = new(_ => _dialogService.CloseOrderConfirmationWindow());
        }
    }
}
