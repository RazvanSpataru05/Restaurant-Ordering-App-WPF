using RestaurantOrderingApp.Display;
using RestaurantOrderingApp.Layers.DataAccessLayer;
using RestaurantOrderingApp.Layers.EntityLayer;
using System.Collections.ObjectModel;

namespace RestaurantOrderingApp.Layers.BusinessLogicLayer
{
    public class MenuBLL
    {
        private readonly MenuDAL _menuDAL = new();

        public ObservableCollection<MenuDisplay> GetAllMenus()
        {
            return _menuDAL.GetAllMenus();
        }
        public ObservableCollection<MenuProduct> GetMenuProducts(int menuId)
        {
            return _menuDAL.GetMenuProducts(menuId);
        }
        public void ClearMenuProducts(int menuId)
        {
            _menuDAL.ClearMenuProducts(menuId);
        }
        public void AddMenuProduct(int menuId, int productId, string portionQuantity)
        {
            _menuDAL.AddMenuProduct(menuId, productId, portionQuantity);
        }
        public Dictionary<int, ObservableCollection<MenuItemDetail>> GetAllMenuItems()
        {
            return _menuDAL.GetAllMenuItems();
        }
    }
}
