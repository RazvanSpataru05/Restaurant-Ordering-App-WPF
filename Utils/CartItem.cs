using RestaurantOrderingApp.Layers.EntityLayer;
using RestaurantOrderingApp.ViewModels;

namespace RestaurantOrderingApp.Utils
{
    public class CartItem : BaseViewModel
    {
        private readonly decimal _unitPrice;

        private int _quantity;
        private decimal _totalPrice;

        public Product? Product { get; set; }
        public Menu? Menu { get; set; }
        public int Quantity
        {
            get => _quantity;
            set { _quantity = value; OnPropertyChanged(nameof(Quantity)); }
        }
        public decimal TotalPrice
        {
            get => _totalPrice;
            set { _totalPrice = value; OnPropertyChanged(nameof(TotalPrice)); }
        }

        public string? DisplayName => Product?.Name ?? Menu?.Name;
        public string? DisplaySubtitle => Product != null
                    ? $"{Product.PortionQuantity} · {Product.Price:F2} RON"
                    : $"Discount {Menu!.DiscountPercent:F0}% · {_unitPrice:F2} RON";

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
    }
}
