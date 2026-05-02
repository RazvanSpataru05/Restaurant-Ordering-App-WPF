

using RestaurantOrderingApp.Layers.DataAccessLayer;
using RestaurantOrderingApp.Layers.EntityLayer;
using System.Collections.ObjectModel;

namespace RestaurantOrderingApp.Layers.BusinessLogicLayer
{
    public class ProductBLL
    {
        private readonly ProductDAL _productDAL = new();

        public ProductBLL() { }

        public ObservableCollection<Product> GetAllProucts()
        {
            return _productDAL.GetAllProducts();
        }
        public ObservableCollection<Product> GetProductsByCategory(int categoryId)
        {
            return _productDAL.GetProductsByCategory(categoryId);
        }
    }
}
