using RestaurantOrderingApp.Layers.DataAccessLayer;
using RestaurantOrderingApp.Layers.EntityLayer;
using System.Collections.ObjectModel;

namespace RestaurantOrderingApp.Layers.BusinessLogicLayer
{
    public class CategoryBLL
    {
        private CategoryDAL _categoryDAL = new();

        public ObservableCollection<Category> GetAllCategories()
        {
            return _categoryDAL.GetAllCategories();
        }
    }
}
