using RestaurantOrderingApp.Dialog_Service;
using RestaurantOrderingApp.Utils;

namespace RestaurantOrderingApp.ViewModels
{
    public class OrderDetailsVM : BaseViewModel
    {
        private readonly IDialogService _dialogService;

        private OrderDisplay _ordersDisplayed;

        public OrderDisplay OrderDisplayed
        {
            get => _ordersDisplayed;
            set
            {
                if (_ordersDisplayed != value)
                {
                    _ordersDisplayed = value;
                    OnPropertyChanged(nameof(OrderDisplayed));
                }
            }
        }
        public RelayCommand CloseCommand { get; set; }

        public OrderDetailsVM(IDialogService dialogService, OrderDisplay orderDisplayed)
        {
            _dialogService = dialogService;
             OrderDisplayed = orderDisplayed;

            CloseCommand = new(_ => _dialogService.CloseOrderDetailsWindow());
        }
    }
}
