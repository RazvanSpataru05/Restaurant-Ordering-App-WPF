using RestaurantOrderingApp.Layers.EntityLayer;

namespace RestaurantOrderingApp.Utils
{
    public class CartItem
    {
        public int Quantity { get; set; }
        public Product Product { get; set; }
        public decimal TotalPrice { get; set; }

        public CartItem(Product product, int quantity)
        {
            Product = product;
            Quantity = quantity;
            TotalPrice = Product.Price * Quantity;
        }
        public void UpdatePrice()
        {
           TotalPrice = Product.Price * Quantity;
        }
    }
}
