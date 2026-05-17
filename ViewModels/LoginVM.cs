using RestaurantOrderingApp.Layers.BusinessLogicLayer;
using RestaurantOrderingApp.Utils;
using RestaurantOrderingApp.Layers.EntityLayer;
using RestaurantOrderingApp.Dialog_Service;
using System.Windows;

namespace RestaurantOrderingApp.ViewModels
{
    public class LoginVM : BaseViewModel
    {
        private readonly UserBLL _userBLL;
        private readonly CurrentUserSession _currentUserSession;
        private readonly IDialogService _dialogService;

        private bool _isLoginVisible;
        private string _email;
        private string _password;
        private string _errorMessage;
        private RegisterVM _registerVM;

        public bool IsLoginVisible
        {
            get => _isLoginVisible;
            set
            {
                _isLoginVisible = value;
                OnPropertyChanged(nameof(IsLoginVisible));
                OnPropertyChanged(nameof(IsRegisterVisible));
            }
        }
        public bool IsRegisterVisible => !IsLoginVisible;
        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(nameof(Email)); }
        }
        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(nameof(Password)); }
        }
        public string ErrorMessage
        {
            get => _errorMessage;
            set { _errorMessage = value; OnPropertyChanged(nameof(ErrorMessage)); }
        }
        public RegisterVM RegisterVM
        {
            get => _registerVM;
            set { _registerVM = value; OnPropertyChanged(nameof(RegisterVM)); }
        }

        public RelayCommand LoginCommand { get; set; }
        public RelayCommand GuestCommand { get; set; }
        public RelayCommand NavigateToRegisterCommand { get; set; }
        public RelayCommand CloseCommand { get; set; }

        public LoginVM(UserBLL userBll, CurrentUserSession currentUserSession, IDialogService dialogService)
        {
            _userBLL = userBll;
            _currentUserSession = currentUserSession;
            _dialogService = dialogService;
            _registerVM = new(_userBLL, () => IsLoginVisible = true);
            IsLoginVisible = true;

            InitializeCommands();
        }

        private void Login()
        {
            if (Email == null || Password == null)
            {
                ErrorMessage = "Email or password fields are empty.";
                return;
            }
            User? user = _userBLL.AuthenticateUser(Email, Password);
            if (user == null)
            {
                ErrorMessage = "Email or password are incorrect";
                return;
            }
            _currentUserSession.CurrentUser = user;
            HandleLogin();
        }
        private void Guest()
        {
            _currentUserSession.CurrentUser = null;
            HandleLogin();
        }
        private void NavigateToRegister()
        {
            IsLoginVisible = false;
        }
        private void HandleLogin()
        {
            _dialogService.ShowRestaurantWindow();
            _dialogService.CloseLoginWindow();
        }
        private void InitializeCommands()
        {
            LoginCommand = new(_ => Login());
            GuestCommand = new(_ => Guest());
            NavigateToRegisterCommand = new(_ => NavigateToRegister());
            CloseCommand = new(_ => Application.Current.Shutdown());
        }
    }
}
