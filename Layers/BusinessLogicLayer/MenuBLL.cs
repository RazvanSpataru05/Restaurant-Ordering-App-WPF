using RestaurantOrderingApp.Layers.DataAccessLayer;
using RestaurantOrderingApp.Layers.EntityLayer;
using System.Collections.ObjectModel;

namespace RestaurantOrderingApp.Layers.BusinessLogicLayer
{
    public class MenuBLL
    {
        private readonly MenuDAL _menuDAL = new();

        public MenuBLL() { }

        public ObservableCollection<Menu> GetAllMenus()
        {
            return _menuDAL.GetAllMenus();
        }
        public ObservableCollection<MenuProduct> GetMenuProducts(int menuId)
        {
            return _menuDAL.GetMenuProducts(menuId);
        }
    }
}
