using RestaurantOrderingApp.Dialog_Service;
using RestaurantOrderingApp.Display;
using RestaurantOrderingApp.Layers.BusinessLogicLayer;
using RestaurantOrderingApp.Utils;
using System.Collections.ObjectModel;

namespace RestaurantOrderingApp.ViewModels
{
    public class AdminPanelVM : BaseViewModel
    {
        private readonly IDialogService _dialogService;
        private readonly OrderBLL _orderBLL;
        private readonly LowStockVM _lowStockVM;

        private ObservableCollection<AdminOrderDisplay> _displayedOrders;
        private bool _showActiveOnly;
        private string _activeView = "Orders";
        private BaseViewModel? _currentAdminView;

        public bool ShowActiveOnly
        {
            get => _showActiveOnly;
            set { _showActiveOnly = value; OnPropertyChanged(nameof(ShowActiveOnly)); }
        }
        public BaseViewModel? CurrentAdminView
        {
            get => _currentAdminView;
            set { _currentAdminView = value; OnPropertyChanged(nameof(CurrentAdminView)); }
        }
        public string ActiveView
        {
            get => _activeView;
            set { _activeView = value; OnPropertyChanged(nameof(ActiveView)); }
        }

        public List<string> AvailableStatuses { get; } = new List<string>() { "Delivered", "Canceled" };
        public ObservableCollection<AdminOrderDisplay> DisplayedOrders
        {
            get => _displayedOrders;
            set { _displayedOrders = value; OnPropertyChanged(nameof(DisplayedOrders)); } 
        }

        public RelayCommand ToggleOrdersCommand { get; set; }
        public RelayCommand ChangeStatusCommand { get; set; }
        public RelayCommand ShowDetailsCommand { get; set; }
        public RelayCommand ShowLowStockCommand { get; set; } 
        public RelayCommand AddProductCommand { get; set; }
        public RelayCommand ManageMenusCommand { get; set; }
        public RelayCommand CloseCommand { get; set; }
        public AdminPanelVM(IDialogService dialogService, OrderBLL orderBLL, LowStockVM lowStockVM)
        {
            _dialogService = dialogService;
            _orderBLL = orderBLL;
            _lowStockVM = lowStockVM;
            CurrentAdminView = null;

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

            return !string.IsNullOrEmpty(adminOrderDisplay.SelectedStatus) &&
                adminOrderDisplay.SelectedStatus != adminOrderDisplay.Order.Status;
        }

        private void ShowDetails(AdminOrderDisplay? adminOrderDisplay)
        {
            if (adminOrderDisplay == null) return;

            _dialogService.ShowOrderDetailsWindow(new OrderDisplay
            {
                Order = adminOrderDisplay.Order,
                Items = adminOrderDisplay.Items
            });
        }
        private void InitializeCommands()
        {
            ToggleOrdersCommand = new(param =>
            {
                if (param is string boolStr && bool.TryParse(boolStr, out bool showActiveOnly))
                {
                    ShowActiveOnly = showActiveOnly;
                    ActiveView = ShowActiveOnly ? "ActiveOnly" : "Orders";
                    CurrentAdminView = null;
                    LoadOrders();
                }
            });

            ChangeStatusCommand = new(param => ChangeStatus(param as AdminOrderDisplay),
                param => CanChangeStatus(param as AdminOrderDisplay));

            ShowDetailsCommand = new(param => ShowDetails(param as AdminOrderDisplay));

            ShowLowStockCommand = new(_ =>
            {
                CurrentAdminView = _lowStockVM;
                ActiveView = "LowStock";
            });
            AddProductCommand = new(_ => _dialogService.ShowAddProductWindow());
            ManageMenusCommand = new(_ => _dialogService.ShowManageMenuWindow());
            CloseCommand = new(_ => _dialogService.ShowWelcomeView());
        }
        public void LoadOrders()
        {
            DisplayedOrders = [];
            var orders = _orderBLL.GetAllOrders();
            if (ShowActiveOnly)
            {
                var filteredOrders = orders.Where(o => o.Order.Status.Trim()
                .Equals("Recorded", StringComparison.OrdinalIgnoreCase)).ToList();
                foreach (var order in filteredOrders)
                {
                    order.Items = _orderBLL.GetOrderItems(order.Order.OrderId);
                    DisplayedOrders.Add(order);
                }
                return;
            }

            foreach (var order in orders)
            {
                order.Items = _orderBLL.GetOrderItems(order.Order.OrderId);
                DisplayedOrders.Add(order);
            }
        }
    }
}
