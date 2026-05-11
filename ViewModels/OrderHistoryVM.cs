using RestaurantOrderingApp.Dialog_Service;
using RestaurantOrderingApp.Layers.BusinessLogicLayer;
using RestaurantOrderingApp.Layers.EntityLayer;
using RestaurantOrderingApp.Utils;
using System.Collections.ObjectModel;

namespace RestaurantOrderingApp.ViewModels
{
    public class OrderHistoryVM : BaseViewModel
    {
        private readonly CurrentUserSession _currentUserSession;
        private readonly OrderBLL _orderBLL;
        private readonly IDialogService _dialogService;

        private ObservableCollection<Order> _orders;
        
        public ObservableCollection<Order> Orders
        {
            get => _orders;
            set
            {
                if (_orders != value)
                {
                    _orders = value;
                    OnPropertyChanged(nameof(Orders));
                }
            }
        }
        public RelayCommand CloseCommand { get; set; }
        public OrderHistoryVM(CurrentUserSession currentUserSession, OrderBLL orderBLL, IDialogService dialogService)
        {
            _currentUserSession = currentUserSession;
            _orderBLL = orderBLL;
            _dialogService = dialogService;

            Orders = _currentUserSession.CurrentUser != null ? _orderBLL.GetOrdersByUser(_currentUserSession.CurrentUser.UserId) : []; 

            CloseCommand = new(_ => _dialogService.ShowWelcomeView());
        }
    }
}
