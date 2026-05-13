using RestaurantOrderingApp.Dialog_Service;
using RestaurantOrderingApp.Layers.BusinessLogicLayer;
using RestaurantOrderingApp.Utils;
using System.Collections.ObjectModel;

namespace RestaurantOrderingApp.ViewModels
{
    public class AdminOrdersVM : BaseViewModel
    {
        private readonly IDialogService _dialogService;
        private readonly OrderBLL _orderBLL;

        private ObservableCollection<AdminOrderDisplay> _displayedOrders;
        private bool _showActiveOnly;

        public bool ShowActiveOnly
        {
            get => _showActiveOnly;
            set
            {
                if (_showActiveOnly != value)
                {
                    _showActiveOnly = value;
                    OnPropertyChanged(nameof(ShowActiveOnly));
                }
            }
        }

        public List<string> AvailableStatuses { get; } = new List<string>() { "Delivered", "Canceled" };
        public ObservableCollection<AdminOrderDisplay> DisplayedOrders
        {
            get => _displayedOrders;
            set
            {
                if (_displayedOrders != value)
                {
                    _displayedOrders = value;
                    OnPropertyChanged(nameof(DisplayedOrders));
                }
            }
        }

        public RelayCommand ToggleOrdersCommand { get; set; }
        public RelayCommand ChangeStatusCommand { get; set; }
        public RelayCommand ShowDetailsCommand { get; set; }
        public RelayCommand CloseCommand { get; set; }
        public AdminOrdersVM(IDialogService dialogService, OrderBLL orderBLL)
        {
            _dialogService = dialogService;
            _orderBLL = orderBLL;

            LoadOrders();
            InitializeCommands();
        }
        private void ChangeStatus(AdminOrderDisplay? adminOrderDisplay)
        {
            if (adminOrderDisplay == null) return;

            _orderBLL.UpdateOrderStatus(adminOrderDisplay.Order.OrderId,
                adminOrderDisplay.SelectedStatus);
            LoadOrders();
        }
        private bool CanChangeStatus(AdminOrderDisplay? adminOrderDisplay)
        {
            if (adminOrderDisplay == null) return false;

            return adminOrderDisplay.SelectedStatus != "Canceled" &&
                adminOrderDisplay.SelectedStatus != "Delivered" &&
                !string.IsNullOrEmpty(adminOrderDisplay.SelectedStatus);
        }
        private void ShowDetails(AdminOrderDisplay? adminOrderDisplay)
        {
            if (adminOrderDisplay == null) return;
        }
        private void InitializeCommands()
        {
            ToggleOrdersCommand = new(_ =>
            {
                ShowActiveOnly = true;
                LoadOrders();
            });
            ChangeStatusCommand = new(param => ChangeStatus(param as AdminOrderDisplay),
                param => CanChangeStatus(param as AdminOrderDisplay));
            ShowDetailsCommand = new(param => ShowDetails(param as AdminOrderDisplay));
            CloseCommand = new(_ => _dialogService.ShowWelcomeView());
        }
        private void LoadOrders()
        {
            DisplayedOrders = [];
            var orders = _orderBLL.GetAllOrders();
            if (ShowActiveOnly)
            {
                orders = new(orders.Where(o => o.Order.Status != "Delivered" &&
                o.Order.Status != "Canceled"));
            }

            foreach (var order in orders)
            {
                order.Items = _orderBLL.GetOrderItems(order.Order.OrderId);
                DisplayedOrders.Add(order);
            }
        }
    }
}
