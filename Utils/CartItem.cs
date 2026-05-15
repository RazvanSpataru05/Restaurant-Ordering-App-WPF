using RestaurantOrderingApp.Layers.EntityLayer;
using System.ComponentModel;

namespace RestaurantOrderingApp.Utils
{
    public class CartItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private readonly decimal _unitPrice;

        private int _quantity;
        private decimal _totalPrice;

        public Product? Product { get; set; }
        public Menu? Menu { get; set; }
        public int Quantity
        {
            get => _quantity;
            set
            {
                if (_quantity != value)
                {
                    _quantity = value;
                    OnPropertyChanged(nameof(Quantity));
                }
            }
        }
        public decimal TotalPrice
        {
            get => _totalPrice;
            set
            {
                if (_totalPrice != value)
                {
                    _totalPrice = value;
                    OnPropertyChanged(nameof(TotalPrice));
                }
            }
        }

        public string? DisplayName => Product?.Name ?? Menu?.Name;

        public CartItem(Product product, int quantity, decimal unitPrice)
        {
            Product = product;
            Quantity = quantity;
            _unitPrice = unitPrice;
            TotalPrice = _unitPrice * Quantity;
        }
        public CartItem(Menu menu, int quantity, decimal unitPrice)
        {
            Menu = menu;
            Quantity = quantity;
            _unitPrice = unitPrice;
            TotalPrice = _unitPrice * Quantity;
        }
        public void UpdatePrice()
        {
            TotalPrice = _unitPrice * Quantity;
        }
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
