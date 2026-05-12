using RestaurantOrderingApp.Dialog_Service;
using RestaurantOrderingApp.Layers.BusinessLogicLayer;
using RestaurantOrderingApp.Utils;
using System.Collections.ObjectModel;

namespace RestaurantOrderingApp.ViewModels
{
    public class OrderHistoryVM : BaseViewModel
    {
        private readonly CurrentUserSession _currentUserSession;
        private readonly OrderBLL _orderBLL;
        private readonly IDialogService _dialogService;

        private ObservableCollection<OrderDisplay> _ordersDisplayed;
        private OrderDisplay? _selectedOrder;
        
        public ObservableCollection<OrderDisplay> OrdersDisplayed
        {
            get => _ordersDisplayed;
            set
            {
                if (_ordersDisplayed != value)
                {
                    _ordersDisplayed = value;
                    OnPropertyChanged(nameof(OrdersDisplayed));
                }
            }
        }
        public OrderDisplay? SelectedOrder
        {
            get => _selectedOrder;
            set
            {
                if (_selectedOrder != value)
                {
                    _selectedOrder = value;
                    OnPropertyChanged(nameof(SelectedOrder));
                }
            }
        }

        public RelayCommand CloseCommand { get; set; }
        public RelayCommand CancelOrderCommand { get; set; }
        public RelayCommand ShowOrderDetailsCommand { get; set; }
        public OrderHistoryVM(CurrentUserSession currentUserSession, OrderBLL orderBLL, IDialogService dialogService)
        {
            _currentUserSession = currentUserSession;
            _orderBLL = orderBLL;
            _dialogService = dialogService;
            SelectedOrder = null;

            InitializeCommands();
        }
        private void CancelOrder()
        {
            if (SelectedOrder == null) return;

            _orderBLL.UpdateOrderStatus(SelectedOrder.Order.OrderId, "Canceled");
            LoadOrders();
        }
        private bool CanOrderBeCanceled()
        {
            if (SelectedOrder == null) return false;

            return SelectedOrder.Order.Status == "Recorded";
        }
        private void ShowOrderDetails(OrderDisplay? orderDisplay)
        {
            if (orderDisplay == null) return;

            _dialogService.ShowOrderDetailsWindow(orderDisplay);
        }
        public void LoadOrders()
        {
            OrdersDisplayed = [];
            if (_currentUserSession.CurrentUser != null)
            {
                var orders = _orderBLL.GetOrdersByUser(_currentUserSession.CurrentUser.UserId);
                foreach (var order in orders)
                {
                    OrdersDisplayed.Add(new OrderDisplay
                    {
                        Order = order,
                        Items = _orderBLL.GetOrderItems(order.OrderId)
                    });
                }
            }
        }
        private void InitializeCommands()
        {
            CloseCommand = new(_ => _dialogService.ShowWelcomeView());
            CancelOrderCommand = new(_ => CancelOrder(), _ => CanOrderBeCanceled());
            ShowOrderDetailsCommand = new(param => ShowOrderDetails(param as OrderDisplay));
        }
    }    
}
