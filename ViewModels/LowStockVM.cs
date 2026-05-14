using RestaurantOrderingApp.Dialog_Service;
using RestaurantOrderingApp.Layers.BusinessLogicLayer;
using RestaurantOrderingApp.Layers.EntityLayer;
using System.Collections.ObjectModel;
using System.Configuration;

namespace RestaurantOrderingApp.ViewModels
{
    public class LowStockVM : BaseViewModel
    {
        private readonly decimal _lowStockPrice = Convert.ToDecimal(ConfigurationManager.AppSettings["LowStockThreshold"]);
        private readonly IDialogService _dialogService;
        private readonly ProductBLL _productBLL;

        private ObservableCollection<Product> _lowStockProducts;

        public ObservableCollection<Product> LowStockProducts
        {
            get => _lowStockProducts;
            set
            {
                if (_lowStockProducts != value)
                {
                    _lowStockProducts = value;
                    OnPropertyChanged(nameof(LowStockProducts));  
                }
            }
        }

        public LowStockVM(IDialogService dialogService, ProductBLL productBLL)
        {
            _dialogService = dialogService;
            _productBLL = productBLL;

            LowStockProducts = _productBLL.GetLowStockProducts(_lowStockPrice);
        }
    }
}
