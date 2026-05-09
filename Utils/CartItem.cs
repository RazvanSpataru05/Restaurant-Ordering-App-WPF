using RestaurantOrderingApp.Layers.EntityLayer;

namespace RestaurantOrderingApp.Utils
{
    public class CartItem
    {
        private readonly decimal _unitPrice;

        public int Quantity { get; set; }
        public Product? Product { get; set; }
        public Menu? Menu { get; set; }
        public decimal TotalPrice { get; set; }

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
