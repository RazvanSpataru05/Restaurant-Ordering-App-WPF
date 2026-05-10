using RestaurantOrderingApp.Dialog_Service;
using RestaurantOrderingApp.Utils;

namespace RestaurantOrderingApp.ViewModels
{
    public class OrderConfirmationVM : BaseViewModel
    {
        private readonly IDialogService _dialogService;

        private string _orderCode;
        private string _estiamtedDelivery;

        public string OrderCode
        {
            get => _orderCode;
            set
            {
                if (_orderCode != value)
                {
                    _orderCode = value;
                    OnPropertyChanged(nameof(OrderCode));
                }
            }
        }
        public string EstimatedDelivery
        {
            get => _estiamtedDelivery;
            set
            {
                if (_estiamtedDelivery != value)
                {
                    _estiamtedDelivery = value;
                    OnPropertyChanged(nameof(EstimatedDelivery));
                }
            }
        }
        public RelayCommand CloseCommand { get; set; }

        public OrderConfirmationVM(IDialogService dialogService, string orderCode, string estimatedDelivery)
        {
            _dialogService = dialogService;
            OrderCode = "ORD-" + orderCode.Substring(0, 9);
            EstimatedDelivery = estimatedDelivery;

            CloseCommand = new(_ => Close());
        }
        private void Close()
        {
            _dialogService.CloseOrderConfirmationWindow();
        }
    }
}
