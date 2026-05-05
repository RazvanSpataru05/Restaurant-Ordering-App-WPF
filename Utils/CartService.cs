
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

       public void AddCartItem(Product product)
        {
            var existingItem = Items.FirstOrDefault(p => p.Product.ProductId == product.ProductId);

            if (existingItem != null)
            {
                existingItem.Quantity += 1;
                existingItem.UpdatePrice();
            }
            else
            {
                Items.Add(new CartItem(product));
            }
        }
    }
}
