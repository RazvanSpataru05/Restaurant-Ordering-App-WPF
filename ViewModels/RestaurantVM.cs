using RestaurantOrderingApp.Dialog_Service;
using RestaurantOrderingApp.Layers.EntityLayer;
using RestaurantOrderingApp.Utils;
using System.Windows;

namespace RestaurantOrderingApp.ViewModels
{
    public class RestaurantVM : BaseViewModel
    {
        private CurrentUserSession _currentUserSession;
        private readonly IDialogService _dialogService;
        private readonly MenuVM _menuVM;
        private readonly CartVM _cartVM;
        private readonly OrderHistoryVM _orderHistoryVM;

        private bool _isProfileMenuOpen;
        private BaseViewModel? _currentView;

        public bool IsLoggedIn => _currentUserSession.CurrentUser != null;
        public bool IsAdmin => _currentUserSession.CurrentUser?.Role == "Employee";
        public CurrentUserSession CurrentUserSession { get; }

        public string DisplayName => _currentUserSession.CurrentUser != null ?
            $"{_currentUserSession.CurrentUser.FirstName} {_currentUserSession.CurrentUser.LastName}" : "Guest";

        public bool IsProfileMenuOpen
        {
            get => _isProfileMenuOpen;
            set { _isProfileMenuOpen = value; OnPropertyChanged(nameof(IsProfileMenuOpen)); }
        }

        public BaseViewModel? CurrentView
        {
            get => _currentView;
            set { _currentView = value; OnPropertyChanged(nameof(CurrentView)); }
        }

        public RelayCommand OpenMenuCommand { get; set; }
        public RelayCommand OpenCartCommand { get; set; }
        public RelayCommand OpenLogoutCommand { get; set; }
        public RelayCommand OpenOrderHistoryCommand { get; set; }
        public RelayCommand ToggleProfileMenuCommand { get; set; }
        public RelayCommand OpenAdminCommand { get; set; }
        public RelayCommand CloseCommand { get; set; }

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
            OpenLogoutCommand = new(_ => _dialogService.ShowLogoutWindow());
            OpenAdminCommand = new(_ => _dialogService.ShowAdminView());
            OpenOrderHistoryCommand = new(_ => OpenOrderHistory());
            ToggleProfileMenuCommand = new(_ => IsProfileMenuOpen = !IsProfileMenuOpen);
            CloseCommand = new(_ => _dialogService.ShowLogoutWindow());
        }
    }
}
