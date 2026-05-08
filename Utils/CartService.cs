using RestaurantOrderingApp.Layers.EntityLayer;
using System.Collections.ObjectModel;

namespace RestaurantOrderingApp.Utils
{
    public class CartService
    {
        public ObservableCollection<CartItem> Items { get; set; }

        public CartService() 
        {
            Items = [];
        }

       public void AddCartItem(Product product, int selectedQuantity)
        {
            var existingItem = Items.FirstOrDefault(p => p.Product.ProductId == product.ProductId);

            if (existingItem != null)
            {
                existingItem.Quantity += selectedQuantity;
                existingItem.UpdatePrice();
            }
            else
            {
                Items.Add(new CartItem(product, selectedQuantity));
            }
        }
        public int GetAvailableProducts(Product product)
        {
            int totalPortions = (int)product.TotalQuantity / ParsePortionQuantity(product.PortionQuantity);
            var cartItem = Items.FirstOrDefault(p => p.Product.ProductId == product.ProductId);
            int alreadyInCart = cartItem?.Quantity ?? 0;
            return totalPortions - alreadyInCart;
        }
        private int ParsePortionQuantity(string portionQuantity)
        {
            int number = 0;
            int index = 0;
            while (char.IsDigit(portionQuantity[index]))
            {
                ++index;
                number = number * 10 + (portionQuantity[index] - '0');
            }
            return number;
        }
    }
}
