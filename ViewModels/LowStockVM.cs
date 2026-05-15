using RestaurantOrderingApp.Dialog_Service;
using RestaurantOrderingApp.Layers.BusinessLogicLayer;
using RestaurantOrderingApp.Layers.EntityLayer;
using System.Collections.ObjectModel;
using System.Configuration;

namespace RestaurantOrderingApp.ViewModels
{
    public class LowStockVM : BaseViewModel
    {
        private readonly decimal _lowStockThreshold = Convert.ToDecimal(ConfigurationManager.AppSettings["LowStockThreshold"]);
        private readonly ProductBLL _productBLL;

        private ObservableCollection<Product> _lowStockProducts;

        public ObservableCollection<Product> LowStockProducts
        {
            get => _lowStockProducts;
            set { _lowStockProducts = value; OnPropertyChanged(nameof(LowStockProducts)); }
        }

        public LowStockVM(ProductBLL productBLL)
        {
            _productBLL = productBLL;

            LowStockProducts = _productBLL.GetLowStockProducts(_lowStockThreshold);
        }
    }
}
