using RestaurantOrderingApp.Dialog_Service;
using RestaurantOrderingApp.Utils;

namespace RestaurantOrderingApp.ViewModels
{
    public class RestaurantVM : BaseViewModel
    {
        private readonly CurrentUserSession _currentUserSession;
        private readonly IDialogService _dialogService;
        private readonly MenuVM _menuVM;
        private readonly CartVM _cartVM;
        private readonly OrderHistoryVM _orderHistoryVM;

        private bool _isProfileMenuOpen;
        private BaseViewModel? _currentView;

        public CurrentUserSession CurrentUserSession { get; }
        public bool IsAdmin => _currentUserSession.CurrentUser?.Role == "Employee";

        public string DisplayName => _currentUserSession.CurrentUser != null ?
            $"{_currentUserSession.CurrentUser.FirstName} {_currentUserSession.CurrentUser.LastName}" : "Guest";

        public bool IsProfileMenuOpen
        {
            get => _isProfileMenuOpen;
            set
            {
                if (_isProfileMenuOpen != value)
                {
                    _isProfileMenuOpen = value;
                    OnPropertyChanged(nameof(IsProfileMenuOpen));
                }
            }
        }

        public BaseViewModel? CurrentView
        {
            get => _currentView;
            set
            {
                if (_currentView != value)
                {
                    _currentView = value;
                    OnPropertyChanged(nameof(CurrentView));
                }
            }
        }

        public RelayCommand OpenMenuCommand { get; set; }
        public RelayCommand OpenCartCommand { get; set; }
        public RelayCommand OpenOrderHistoryCommand { get; set; }
        public RelayCommand ToggleProfileMenuCommand { get; set; }
        public RelayCommand OpenAdminCommand { get; set; }

        public RestaurantVM(CurrentUserSession currentUserSession, IDialogService dialogService, MenuVM menuVM,
            CartVM cartVM, OrderHistoryVM orderHistoryVM)
        {
            _currentUserSession = currentUserSession;
            _dialogService = dialogService;
            _menuVM = menuVM;
            _cartVM = cartVM;
            _orderHistoryVM = orderHistoryVM;
            IsProfileMenuOpen = false;

            InitializeCommands();
        }
        private void OpenCart()
        {
            CurrentView = _cartVM;
            foreach (var menuItem in _menuVM.FullMenu)
            {
                foreach (var product in menuItem.Products)
                {
                    product.SelectedQuantity = 1;
                }
            }
        }
        private void OpenOrderHistory()
        {
            if (_currentUserSession.CurrentUser == null)
            {
                _dialogService.ShowGuestWarningWindow("You must be logged in to see your order history!");
                return;
            }
            _orderHistoryVM.LoadOrders();
            CurrentView = _orderHistoryVM;
        }
        private void InitializeCommands()
        {
            OpenMenuCommand = new(_ => CurrentView = _menuVM);
            OpenCartCommand = new(_ => OpenCart());
            OpenOrderHistoryCommand = new(_ => OpenOrderHistory());
            ToggleProfileMenuCommand = new(_ => IsProfileMenuOpen = !IsProfileMenuOpen);
        }
    }
}
