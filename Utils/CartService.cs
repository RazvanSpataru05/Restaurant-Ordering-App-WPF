using RestaurantOrderingApp.Layers.BusinessLogicLayer;
using RestaurantOrderingApp.Layers.EntityLayer;
using System.Collections.ObjectModel;

namespace RestaurantOrderingApp.Utils
{
    public class CartService
    {
        private readonly MenuBLL _menuBLL;
        public ObservableCollection<CartItem> Items { get; set; }

        public CartService(MenuBLL menuBLL)
        {
            _menuBLL = menuBLL;
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
                Items.Add(new CartItem(product, selectedQuantity, product.Price));
            }
        }
        public int GetAvailablePortions(Product product, CartItem? excludeItem = null)
        {
            int totalPortions = ComputeTotalPortions(product.PortionQuantity, product.TotalQuantity);
            var cartItem = Items.FirstOrDefault(p => p.Product?.ProductId == product.ProductId && p != excludeItem);
            int alreadyInCart = cartItem?.Quantity ?? 0;
            return totalPortions - alreadyInCart;
        }
        public int GetAvailablePortions(Menu menu)
        {
            var menuProducts = _menuBLL.GetMenuProducts(menu.MenuId);
            int minimumPortions = int.MaxValue;
            foreach (var menuProduct in menuProducts)
            {
                int totalProductPortions = ComputeTotalPortions(menuProduct.PortionQuantity, menuProduct.TotalQuantity);
                var productCartItem = Items.FirstOrDefault(i => i.Product?.ProductId == menuProduct.ProductId);
                int usedByProducts = productCartItem != null
                    ? productCartItem.Quantity * ParsePortionQuantity(productCartItem.Product!.PortionQuantity)
                    : 0;

                int usedByMenus = 0;
                foreach (var cartItem in Items.Where(i => i.Menu != null))
                {
                    var menuProductInCart = _menuBLL.GetMenuProducts(cartItem.Menu!.MenuId);
                    var match = menuProductInCart.FirstOrDefault(mp => mp.ProductId == menuProduct.ProductId);
                    if (match != null)
                    {
                        usedByMenus += cartItem.Quantity * ParsePortionQuantity(match.PortionQuantity);
                    }
                }

                int remaining = (int)(menuProduct.TotalQuantity - (decimal)(usedByProducts + usedByMenus));
                int availableMenus = (int)(remaining / ParsePortionQuantity(menuProduct.PortionQuantity));

                if (availableMenus < minimumPortions)
                {
                    minimumPortions = availableMenus;
                }
            }
            return Math.Max(0, minimumPortions);
        }

        public int ParsePortionQuantity(string portionQuantity)
        {
            int number = 0;
            int index = 0;
            while (char.IsDigit(portionQuantity[index]))
            {
                number = number * 10 + (portionQuantity[index] - '0');
                ++index;
            }
            return number;
        }
        private int ComputeTotalPortions(string portionQuantity, decimal totalQuantity)
        {
            return (int)totalQuantity / ParsePortionQuantity(portionQuantity);
        }
    }
}
