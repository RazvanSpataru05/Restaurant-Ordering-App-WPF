using RestaurantOrderingApp.Dialog_Service;
using RestaurantOrderingApp.Utils;

namespace RestaurantOrderingApp.ViewModels
{
    public class RestaurantVM : BaseViewModel
    {
        private readonly CurrentUserSession _currentUserSession;
        private readonly IDialogService _dialogService;

        private MenuVM _menuVM;
        private CartVM _cartVM;

        private bool _isMenuOpen;
        private bool _isCartOpen;
        private bool _isWelcomeVisible;

        public CurrentUserSession CurrentUserSession
        {
            get => _currentUserSession;
        }

        public string DisplayName => _currentUserSession.CurrentUser != null ?
            $"{_currentUserSession.CurrentUser.FirstName} {_currentUserSession.CurrentUser.LastName}" : "Guest";

        public MenuVM MenuVM
        {
            get => _menuVM;
            set
            {
                if (_menuVM != value)
                {
                    _menuVM = value;
                    OnPropertyChanged(nameof(MenuVM));
                }
            }
        }
        public CartVM CartVM
        {
            get => _cartVM;
            set
            {
                if (_cartVM != value)
                {
                    _cartVM = value;
                    OnPropertyChanged(nameof(CartVM));
                }
            }
        }

        public bool IsMenuOpen
        {
            get => _isMenuOpen;
            set
            {
                if (_isMenuOpen != value)
                {
                    _isMenuOpen = value;
                    OnPropertyChanged(nameof(IsMenuOpen));
                }
            }
        }
        public bool IsCartOpen
        {
            get => _isCartOpen;
            set
            {
                if (_isCartOpen != value)
                {
                    _isCartOpen = value;
                    OnPropertyChanged(nameof(IsCartOpen));
                }
            }
        }
        public bool IsWelcomeVisible
        {
            get => _isWelcomeVisible;
            set
            {
                if (_isWelcomeVisible != value)
                {
                    _isWelcomeVisible = value;
                    OnPropertyChanged(nameof(IsWelcomeVisible));
                }
            }
        }

        public RelayCommand OpenMenuCommand { get; set; }
        public RelayCommand OpenCartCommand { get; set; }

        public RestaurantVM(CurrentUserSession currentUserSession, IDialogService dialogService, MenuVM menuVM, CartVM cartVM)
        {
            _currentUserSession = currentUserSession;
            _dialogService = dialogService;
            _menuVM = menuVM;
            _cartVM = cartVM;

            IsWelcomeVisible = true;
            IsMenuOpen = false;
            IsCartOpen = false;

            OpenMenuCommand = new(_ => OpenMenu());
            OpenCartCommand = new(_ => OpenCart());
        }
        private void OpenMenu()
        {
            IsMenuOpen = true;
            IsCartOpen = false;
            IsWelcomeVisible = false;
        }
        private void OpenCart()
        {
            IsCartOpen = true;
            IsMenuOpen = false;
            IsWelcomeVisible = false;
        }
    }
}
