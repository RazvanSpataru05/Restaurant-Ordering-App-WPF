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
        public AdminOrdersVM(IDialogService dialogService, OrderBLL orderBLL)
        {
            _dialogService = dialogService;
            _orderBLL = orderBLL;

            InitializeCommands();
        }
        private void ChangeStatus(AdminOrderDisplay? adminOrderDisplay)
        {
            if (adminOrderDisplay == null) return;

            _orderBLL.UpdateOrderStatus(adminOrderDisplay.OrderDisplay.Order.OrderId,
                adminOrderDisplay.SelectedStatus);
        }
        private bool CanChangeStatus(AdminOrderDisplay? adminOrderDisplay)
        {
            if (adminOrderDisplay == null) return false;

            return adminOrderDisplay.OrderDisplay.Order.Status != "Canceled" &&
                adminOrderDisplay.OrderDisplay.Order.Status != "Delivered";
        }
        private void ShowDetails(AdminOrderDisplay? adminOrderDisplay)
        {
            if (adminOrderDisplay == null) return;
        }
        private void InitializeCommands()
        {
            ToggleOrdersCommand = new(_ => ShowActiveOnly = !ShowActiveOnly);
            ChangeStatusCommand = new(param => ChangeStatus(param as AdminOrderDisplay),
                param => CanChangeStatus(param as AdminOrderDisplay));
            ShowDetailsCommand = new(param => ShowDetails(param as AdminOrderDisplay));
        }
    }
}
