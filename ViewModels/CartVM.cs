using RestaurantOrderingApp.Dialog_Service;
using RestaurantOrderingApp.Layers.BusinessLogicLayer;
using RestaurantOrderingApp.Layers.EntityLayer;
using RestaurantOrderingApp.Utils;
using System.Configuration;
using System.Security.Policy;

namespace RestaurantOrderingApp.ViewModels
{
    public class CartVM : BaseViewModel
    {
        private readonly Random _random = new();
        private readonly IDialogService _dialogService;
        private readonly OrderBLL _orderBLL;
        private readonly CurrentUserSession _currentUserSession;
        private readonly CartService _cartService;
        private readonly int _menuDiscount;
        private readonly int _orderDiscountThreshold;
        private readonly int _frequencyDays;
        private readonly int _frequencyOrderCount;
        private readonly decimal _frequencyDiscount;
        private readonly int _freeDeliveryThreshold;
        private readonly int _configDeliveryCost;


        private bool _isCartEmpty;
        private string _cartMessage;
        private decimal _subtotal;
        private decimal _discountAmount;
        private int _deliveryCost;
        private decimal _total;

        public bool IsCartEmpty
        {
            get => _isCartEmpty;
            set
            {
                if (_isCartEmpty != value)
                {
                    _isCartEmpty = value;
                    OnPropertyChanged(nameof(IsCartEmpty));
                }
            }
        }
        public string CartMessage
        {
            get => _cartMessage;
            set
            {
                if (_cartMessage != value)
                {
                    _cartMessage = value;
                    OnPropertyChanged(nameof(CartMessage));
                }
            }
        }
        public decimal Subtotal
        {
            get => _subtotal;
            set
            {
                if (_subtotal != value)
                {
                    _subtotal = value;
                    OnPropertyChanged(nameof(Subtotal));
                }
            }
        }
        public decimal DiscountAmount
        {
            get => _discountAmount;
            set
            {
                if (_discountAmount != value)
                {
                    _discountAmount = value;
                    OnPropertyChanged(nameof(DiscountAmount));
                }
            }
        }
        public int DeliveryCost
        {
            get => _deliveryCost;
            set
            {
                if (_deliveryCost != value)
                {
                    _deliveryCost = value;
                    OnPropertyChanged(nameof(DeliveryCost));
                }
            }
        }
        public decimal Total
        {
            get => _total;
            set
            {
                if (_total != value)
                {
                    _total = value;
                    OnPropertyChanged(nameof(Total));
                }
            }
        }

        public RelayCommand PlaceOrderCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }
        public RelayCommand RemoveFromCartCommand { get; set; }
        public RelayCommand IncreaseCommand { get; set; }
        public RelayCommand DecreaseCommand { get; set; }
        public CartVM(IDialogService dialogService, OrderBLL orderBLL, CurrentUserSession currentUserSession, CartService cartService)
        {
            _dialogService = dialogService;
            _orderBLL = orderBLL;
            _currentUserSession = currentUserSession;
            _cartService = cartService;
            _menuDiscount = Convert.ToInt32(ConfigurationManager.AppSettings["MenuDiscount"]);
            _orderDiscountThreshold = Convert.ToInt32(ConfigurationManager.AppSettings["OrderDiscountThreshold"]);
            _frequencyDays = Convert.ToInt32(ConfigurationManager.AppSettings["FrequencyDays"]);
            _frequencyOrderCount = Convert.ToInt32(ConfigurationManager.AppSettings["FrequencyOrderCount"]);
            _frequencyDiscount = Convert.ToDecimal(ConfigurationManager.AppSettings["FrequencyDiscount"]);
            _freeDeliveryThreshold = Convert.ToInt32(ConfigurationManager.AppSettings["FreeDeliveryThreshold"]);
            _configDeliveryCost = Convert.ToInt32(ConfigurationManager.AppSettings["DeliveryCost"]);

            IsCartEmpty = _cartService.Items.Count == 0;
            if (IsCartEmpty) CartMessage = "Your cart is currently empty.";

            ComputeTotal();
            PlaceOrderCommand = new(_ => PlaceOrder());
            CancelCommand = new(_ => Cancel());
            RemoveFromCartCommand = new(param => RemoveFromCart(param as CartItem));
            IncreaseCommand = new(param => Increase(param as CartItem), param => CanIncrease(param as CartItem));
            DecreaseCommand = new(param => Decrease(param as CartItem), param => CanDecrease(param as CartItem));
        }
        private void PlaceOrder()
        {
            if (IsCartEmpty || _currentUserSession.CurrentUser == null) return;
            int orderId = _orderBLL.CreateOrder(_currentUserSession.CurrentUser.UserId, GenerateDateTime(), Total, _configDeliveryCost);
            foreach (var item in _cartService.Items)
            {
                _orderBLL.AddOrderItem(orderId, item.Product?.ProductId, item.Menu?.MenuId, item.Quantity, item.TotalPrice);
            }
            _cartService.Items.Clear();
        }
        private DateTime GenerateDateTime()
        {
            int randomHours = _random.Next(0, 3);
            int randomMinutes = _random.Next(1, 61);
            return DateTime.Now.AddMinutes(randomHours * 60 + randomMinutes);
        }
        private void Cancel()
        {
            _dialogService.ShowWelcomeView();
        }
        private void RemoveFromCart(CartItem? cartItem)
        {
            if (cartItem == null) return;

            _cartService.Items.Remove(cartItem);
            ComputeTotal();
        }
        private void Increase(CartItem? cartItem)
        {
            if (cartItem == null) return;

            cartItem.Quantity += 1;
            ComputeTotal();
        }
        private bool CanIncrease(CartItem? cartItem)
        {
            if (cartItem == null) return false;

            return cartItem.Quantity < _cartService.GetAvailablePortions(cartItem.Product);
        }
        private void Decrease(CartItem? cartItem)
        {
            if (cartItem == null) return;

            cartItem.Quantity -= 1;
            ComputeTotal();
        }
        private bool CanDecrease(CartItem? cartItem)
        {
            if (cartItem == null) return false;

            return cartItem.Quantity > 1;
        }
        private void ComputeTotal()
        {
            Subtotal = _cartService.Items.Sum(i => i.TotalPrice);
            Total = Subtotal;
            DeliveryCost = IsDeliveryFree() ? 0 : _configDeliveryCost;
            if (Subtotal >= _orderDiscountThreshold || IsFrequencyDiscountEligible())
            {
                DiscountAmount = (_frequencyDiscount / 100) * Subtotal;
                Total = Subtotal - (_frequencyDiscount / 100) * Subtotal;
            }
            if (!IsDeliveryFree())
            {
                Total += _configDeliveryCost;
            }
        }
        private bool IsFrequencyDiscountEligible()
        {
            if (_currentUserSession.CurrentUser == null) return false;

            var userOrders = _orderBLL.GetOrdersByUser(_currentUserSession.CurrentUser.UserId);
            int eligibleOrders = userOrders.Count(o => (DateTime.Now - o.OrderDate).TotalDays < _frequencyDays);
            return eligibleOrders >= _frequencyOrderCount;
        }
        private bool IsDeliveryFree()
        {
            return Subtotal >= _freeDeliveryThreshold;
        }
    }
}
