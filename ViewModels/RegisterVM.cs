using RestaurantOrderingApp.Dialog_Service;
using RestaurantOrderingApp.Layers.BusinessLogicLayer;
using RestaurantOrderingApp.Layers.EntityLayer;
using RestaurantOrderingApp.Utils;
using System.Windows;

namespace RestaurantOrderingApp.ViewModels
{
    public class RegisterVM : BaseViewModel
    {
        private readonly IDialogService _dialogService;
        private readonly UserBLL _userBLL;
        private readonly Action _onBackToLogin;

        private string _firstName;
        private string _lastName;
        private string _email;
        private string _password;
        private string _confirmPassword;
        private string _phone;
        private string _deliveryAddress;
        private string _errorMessage;

        public event Action OnClearPasswords;

        public string FirstName
        {
            get => _firstName;
            set
            {
                if (_firstName != value)
                {
                    _firstName = value;
                    OnPropertyChanged(nameof(FirstName));
                }
            }
        }
        public string LastName
        {
            get => _lastName;
            set
            {
                if (_lastName != value)
                {
                    _lastName = value;
                    OnPropertyChanged(nameof(LastName));
                }
            }
        }
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
        public string ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                if (_confirmPassword != value)
                {
                    _confirmPassword = value;
                    OnPropertyChanged(nameof(ConfirmPassword));
                }
            }
        }

        public string Phone
        {
            get => _phone;
            set
            {
                if (_phone != value)
                {
                    _phone = value;
                    OnPropertyChanged(nameof(Phone));
                }
            }
        }
        public string DeliveryAddress
        {
            get => _deliveryAddress;
            set
            {
                if (_deliveryAddress != value)
                {
                    _deliveryAddress = value;
                    OnPropertyChanged(nameof(DeliveryAddress));
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

        public RelayCommand RegisterCommand { get; set; }
        public RelayCommand BackToLoginCommand { get; set; }

        public RegisterVM(IDialogService dialogService, UserBLL userBLL, Action onBackToLogin)
        {
            _dialogService = dialogService;
            _userBLL = userBLL;
            _onBackToLogin = onBackToLogin;

            InitializeCommands();
        }

        private void Register()
        {
            if (FirstName == null || LastName == null)
            {
                ErrorMessage = "Name cannot be empty.";
                return;
            }
            if (!AuthenticationValidator.IsEmailValid(Email))
            {
                ErrorMessage = "Email format is not valid.";
                return;
            }
            if (!AuthenticationValidator.IsPasswordValid(Password))
            {
                ErrorMessage = "Password is not strong enough.";
                return;
            }
            if (Password != ConfirmPassword)
            {
                ErrorMessage = "Confirmation password different from initial password.";
                return;
            }
            if (!AuthenticationValidator.IsPhoneValid(Phone))
            {
                ErrorMessage = "Phone number is not valid.";
                return;
            }
            if (!AuthenticationValidator.IsDeliveryAddressValid(DeliveryAddress))
            {
                ErrorMessage = "Delivery address is not valid.";
                return;
            }
            if (_userBLL.RegisterUser(new User
            {
                FirstName = FirstName,
                LastName = LastName,
                Email = Email,
                Phone = Phone,
                DeliveryAddress = DeliveryAddress,
                PasswordHash = CryptingHelper.HashPassword(Password)
            }))
            {
                MessageBox.Show("Account has been created successfully", "Inforamtion",
                 MessageBoxButton.OK,
                 MessageBoxImage.Information);
                 ClearFields();
                _onBackToLogin();
            }
            else
            {
                ErrorMessage = "Email is already used by another user.";
            }
        }
        private void BackToLogin()
        {
            _onBackToLogin();
        }
        private void ClearFields()
        {
            FirstName = "";
            LastName = "";
            Email = "";
            Phone = "";
            Password = "";
            ConfirmPassword = "";
            DeliveryAddress = "";
            ErrorMessage = "";
            OnClearPasswords?.Invoke();
        }
        private void InitializeCommands()
        {
            RegisterCommand = new(_ => Register());
            BackToLoginCommand = new(_ => BackToLogin());
        }
    }
}
