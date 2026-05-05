using RestaurantOrderingApp.Layers.EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantOrderingApp.Utils
{
    public class CartItem
    {
        public int Quantity { get; set; }
        public Product Product { get; set; }
        public decimal TotalPrice { get; set; }

        public CartItem(Product product)
        {
            Product = product;
            Quantity = 1;
            TotalPrice = Product.Price;
        }
        public void UpdatePrice()
        {
           TotalPrice = Product.Price * Quantity;
        }
    }
}
