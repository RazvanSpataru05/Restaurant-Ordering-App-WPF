using RestaurantOrderingApp.Dialog_Service;
using RestaurantOrderingApp.Layers.BusinessLogicLayer;
using RestaurantOrderingApp.Layers.EntityLayer;
using RestaurantOrderingApp.Utils;
using System.Collections.ObjectModel;
using System.Configuration;

namespace RestaurantOrderingApp.ViewModels
{
    public class CartVM : BaseViewModel
    {
        private readonly int _menuDiscount = Convert.ToInt32(ConfigurationManager.AppSettings["MenuDiscount"]);
        private readonly int _orderDiscountThreshold = Convert.ToInt32(ConfigurationManager.AppSettings["OrderDiscountThreshold"]);
        private readonly int _frequencyDays = Convert.ToInt32(ConfigurationManager.AppSettings["FrequencyDays"]);
        private readonly int _frequencyOrderCount = Convert.ToInt32(ConfigurationManager.AppSettings["FrequencyOrderCount"]);
        private readonly decimal _frequencyDiscount = Convert.ToDecimal(ConfigurationManager.AppSettings["FrequencyDiscount"]);
        private readonly int _freeDeliveryThreshold = Convert.ToInt32(ConfigurationManager.AppSettings["FreeDeliveryThreshold"]);
        private readonly int _configDeliveryCost = Convert.ToInt32(ConfigurationManager.AppSettings["DeliveryCost"]);

        private readonly Random _random = new();
        private readonly IDialogService _dialogService;
        private readonly OrderBLL _orderBLL;
        private readonly ProductBLL _productBLL;
        private readonly MenuBLL _menuBLL;
        private readonly CurrentUserSession _currentUserSession;
        private readonly CartService _cartService;

        private bool _isCartEmpty;
        private string _cartMessage;
        private decimal _subtotal;
        private decimal _discountAmount;
        private int _deliveryCost;
        private decimal _total;

        public ObservableCollection<CartItem> CartItems => _cartService.Items;

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
        public CartVM(IDialogService dialogService, OrderBLL orderBLL, ProductBLL productBLL, MenuBLL menuBLL,
            CurrentUserSession currentUserSession, CartService cartService)
        {
            _dialogService = dialogService;
            _orderBLL = orderBLL;
            _productBLL = productBLL;
            _menuBLL = menuBLL;
            _currentUserSession = currentUserSession;
            _cartService = cartService;

            IsCartEmpty = _cartService.Items.Count == 0;
            CartMessage = IsCartEmpty ? "Your cart is currently empty" : string.Empty;

            _cartService.Items.CollectionChanged += (s, e) =>
            {
                IsCartEmpty = _cartService.Items.Count == 0;
                CartMessage = IsCartEmpty ? "Your cart is currently empty" : string.Empty;
                ComputeTotal();
            };

            ComputeTotal();
            InitializeCommands();
        }
        private void PlaceOrder()
        {
            if (IsCartEmpty || _currentUserSession.CurrentUser == null) return;

            DateTime estimatedDeliveryTime = GenerateDateTime();
            (int orderId, string orderCode) = _orderBLL.CreateOrder
                (_currentUserSession.CurrentUser.UserId, estimatedDeliveryTime, Total, _configDeliveryCost);
            Dictionary<int, decimal> quantitiesToUpdate = [];

            foreach (var item in _cartService.Items)
            {
                _orderBLL.AddOrderItem(orderId, item.Product?.ProductId, item.Menu?.MenuId, item.Quantity, item.TotalPrice);
                if (item.Product != null)
                {
                    decimal remainingQuantity = item.Product.TotalQuantity - item.Quantity
                        * _cartService.ParsePortionQuantity(item.Product.PortionQuantity);
                    quantitiesToUpdate.Add(item.Product.ProductId, remainingQuantity);
                }
                else if (item.Menu != null)
                {
                    var menuProducts = _menuBLL.GetMenuProducts(item.Menu.MenuId);
                    foreach (var menuProduct in menuProducts)
                    {
                        decimal used = item.Quantity * _cartService.ParsePortionQuantity(menuProduct.PortionQuantity);
                        if (quantitiesToUpdate.ContainsKey(menuProduct.ProductId))
                        {
                            quantitiesToUpdate[menuProduct.ProductId] -= used;
                        }
                        else
                        {
                            quantitiesToUpdate.Add(menuProduct.ProductId, menuProduct.TotalQuantity - used);
                        }
                    }
                }
            }
            foreach (var key in quantitiesToUpdate.Keys)
            {
                _productBLL.UpdateProductQuantity(key, quantitiesToUpdate[key]);
            }
            SetupOrder(orderId, orderCode, estimatedDeliveryTime);
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
        }
        private void Increase(CartItem? cartItem)
        {
            if (cartItem == null) return;

            cartItem.Quantity += 1;
            cartItem.UpdatePrice();
            ComputeTotal();
        }
        private bool CanIncrease(CartItem? cartItem)
        {
            if (cartItem == null) return false;

            if (cartItem.Product != null) return cartItem.Quantity < _cartService.GetAvailablePortions(cartItem.Product, cartItem);
            return cartItem.Quantity < _cartService.GetAvailablePortions(cartItem.Menu!);
        }
        private void Decrease(CartItem? cartItem)
        {
            if (cartItem == null) return;

            cartItem.Quantity -= 1;
            cartItem.UpdatePrice();
            ComputeTotal();
        }
        private bool CanDecrease(CartItem? cartItem)
        {
            if (cartItem == null) return false;

            return cartItem.Quantity > 1;
        }
        private void ComputeTotal()
        {
            if (IsCartEmpty) return;

            Subtotal = _cartService.Items.Sum(i => i.TotalPrice);
            Total = Subtotal;
            DeliveryCost = IsDeliveryFree() ? 0 : _configDeliveryCost;
            if (Subtotal >= _orderDiscountThreshold || IsFrequencyDiscountEligible())
            {   
                DiscountAmount = (_frequencyDiscount / 100) * Subtotal;
                Total = Subtotal - (_frequencyDiscount / 100) * Subtotal;
            }
            else
            {
                DiscountAmount = 0;
            }
            Total += DeliveryCost;
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
        private void InitializeCommands()
        {
            PlaceOrderCommand = new(_ => PlaceOrder(), _ => IsCartEmpty == false);
            CancelCommand = new(_ => Cancel());
            RemoveFromCartCommand = new(param => RemoveFromCart(param as CartItem));
            IncreaseCommand = new(param => Increase(param as CartItem), param => CanIncrease(param as CartItem));
            DecreaseCommand = new(param => Decrease(param as CartItem), param => CanDecrease(param as CartItem));
        }
        private void SetupOrder(int orderId, string orderCode, DateTime estimatedDeliveryTime)
        {
            var orderItems = _orderBLL.GetOrderItems(orderId);
            var order = new Order
            {
                OrderId = orderId,
                OrderCode = orderCode,
                OrderDate = DateTime.Now,
                EstimatedDeliveryTime = estimatedDeliveryTime,
                TotalPrice = Total,
                Status = "Recorded"
            };
            ResetOrderCosts();
            _dialogService.ShowOrderConfirmationWindow(orderCode, estimatedDeliveryTime.ToString("HH:mm"));
            _dialogService.ShowOrderHistoryView();
            _dialogService.ShowOrderDetailsWindow(new OrderDisplay { Order = order, Items = orderItems });
        }
        private void ResetOrderCosts()
        {
            _cartService.Items.Clear();
            Subtotal = 0;
            DeliveryCost = 0;
            Total = 0;
            DiscountAmount = 0;
        }
    }
}
