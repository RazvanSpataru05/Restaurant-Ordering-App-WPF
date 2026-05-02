using RestaurantOrderingApp.Layers.BusinessLogicLayer;
using RestaurantOrderingApp.Utils;
using RestaurantOrderingApp.Layers.EntityLayer;
using RestaurantOrderingApp.Dialog_Service;

namespace RestaurantOrderingApp.ViewModels
{
    public class LoginVM : BaseViewModel
    {
        private readonly UserBLL _userBLL;
        private readonly CurrentUserSession _session;
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
                if (_isLoginVisible != value)
                {
                    _isLoginVisible = value;
                    OnPropertyChanged(nameof(IsLoginVisible));
                    OnPropertyChanged(nameof(IsRegisterVisible));
                }
            }
        }
        public bool IsRegisterVisible => !IsLoginVisible;
        public string Email
        {
            get => _email;
            set
            {
                if (_email != value)
                {
                    _email = value;
                    OnPropertyChanged(nameof(Email));
                }
            }
        }
        public string Password
        {
            get => _password;
            set
            {
                if (_password != value)
                {
                    _password = value;
                    OnPropertyChanged(nameof(Password));
                }
            }
        }
        public string ErrorMessage
        {
            get => _errorMessage; 
            set
            {
                if (_errorMessage != value)
                {
                    _errorMessage = value;
                    OnPropertyChanged(nameof(ErrorMessage)); 
                }
            }
        }
        public RegisterVM RegisterVM
        {
            get => _registerVM;
            set
            {
                if (_registerVM != value)
                {
                    _registerVM = value;
                    OnPropertyChanged(nameof(RegisterVM));
                }
            }
        }

        public RelayCommand LoginCommand { get; set; }
        public RelayCommand GuestCommand { get; set; }
        public RelayCommand NavigateToRegisterCommand { get; set; }

        public LoginVM(UserBLL userBll, CurrentUserSession session, IDialogService dialogService)
        {
            _userBLL = userBll;
            _session = session;
            _dialogService = dialogService;
            _registerVM = new(_dialogService, _userBLL, () => IsLoginVisible = true);
            IsLoginVisible = true;

            LoginCommand = new(_ => Login());
            GuestCommand = new(_ => Guest());
            NavigateToRegisterCommand = new(_ => NavigateToRegister());
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
            _session.CurrentUser = user;
            HandleLogin();
        }
        private void Guest()    
        {
            _session.CurrentUser = null;
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
    }
}
