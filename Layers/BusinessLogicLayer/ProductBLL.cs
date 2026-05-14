using RestaurantOrderingApp.Layers.DataAccessLayer;
using RestaurantOrderingApp.Layers.EntityLayer;
using System.Collections.ObjectModel;

namespace RestaurantOrderingApp.Layers.BusinessLogicLayer
{
    public class ProductBLL
    {
        private readonly ProductDAL _productDAL = new();

        public ObservableCollection<Product> GetAllProucts()
        {
            return _productDAL.GetAllProducts();
        }
        public ObservableCollection<Product> GetProductsByCategory(int categoryId)
        {
            return _productDAL.GetProductsByCategory(categoryId);
        }
        public void UpdateProductQuantity(int productId, decimal totalQuantity)
        {
            _productDAL.UpdateProductQuantity(productId, totalQuantity);
        }
        public ObservableCollection<Product> GetLowStockProducts(decimal lowStock)
        {
            return _productDAL.GetLowStockProducts(lowStock);
        }
        public bool AddProduct(string name, int categoryId, string portionQuantity,
            decimal totalQuantity, decimal price, string imagePath)
        {
            return _productDAL.AddProduct(name, categoryId, portionQuantity, totalQuantity, price, imagePath);
        }
    }
}
