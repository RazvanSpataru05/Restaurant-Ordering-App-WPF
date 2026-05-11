using RestaurantOrderingApp.Layers.DataAccessLayer;
using RestaurantOrderingApp.Layers.EntityLayer;
using System.Collections.ObjectModel;

namespace RestaurantOrderingApp.Layers.BusinessLogicLayer
{
    public class AllergenBLL
    {
        private readonly AllergenDAL _allergenDAL = new();

        public ObservableCollection<Allergen> GetAllAllergens()
        {
            return _allergenDAL.GetAllAllergens();
        }
        public ObservableCollection<Allergen> GetProductAllergens(int productId)
        {
            return _allergenDAL.GetProductAllergens(productId);
        }
        public Dictionary<int, ObservableCollection<Allergen>> GetAllProductAllergens()
        {
            return _allergenDAL.GetAllProductAllergens();
        }
    }
}
