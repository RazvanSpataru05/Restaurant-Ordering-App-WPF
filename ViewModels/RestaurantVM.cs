using RestaurantOrderingApp.Dialog_Service;
using RestaurantOrderingApp.Layers.BusinessLogicLayer;
using RestaurantOrderingApp.Layers.EntityLayer;
using RestaurantOrderingApp.Utils;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace RestaurantOrderingApp.ViewModels
{
    public class RestaurantVM : BaseViewModel
    {
        private readonly CurrentUserSession _currentUserSession;
        private readonly ProductBLL _productBLL;
        private readonly CategoryBLL _categoryBLL;
        private readonly IDialogService _dialogService;

        private ObservableCollection<CategoryWithProducts> _fullMenu;
        private ObservableCollection<CategoryWithProducts> _filteredMenu;
        private string _searchText;

        public RelayCommand SearchCommand { get; set; }
        public RelayCommand PlaceOrderCommand { get; set; }

        public ObservableCollection<CategoryWithProducts> FullMenu
        {
            get { return _fullMenu; }
            set
            {
                if (_fullMenu != value)
                {
                    _fullMenu = value;
                    OnPropertyChanged(nameof(FullMenu));
                }
            }
        }

        public ObservableCollection<CategoryWithProducts> FilteredMenu
        {
            get { return _filteredMenu; }
            set
            {
                if (_filteredMenu != value)
                {
                    _filteredMenu = value;
                    OnPropertyChanged(nameof(FilteredMenu));
                }
            }
        }

        public string SearchText
        {
            get { return _searchText; }
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    OnPropertyChanged(nameof(SearchText));
                }
            }
        }
        public RestaurantVM(CurrentUserSession currentUserSession, ProductBLL productBLL, CategoryBLL categoryBLL, IDialogService dialogService)
        {
            _currentUserSession = currentUserSession;
            _productBLL = productBLL;
            _categoryBLL = categoryBLL;
            _dialogService = dialogService;
            _fullMenu = [];
            _filteredMenu = [];

            var allProducts = _productBLL.GetAllProucts();
            var grouped = allProducts
                .GroupBy(p => p.CategoryName)
                .Select(g => new CategoryWithProducts(
                    category: g.Key,
                    products: new(g.ToList())));
            _fullMenu = new(grouped);

            SearchCommand = new(_ => Search());
            PlaceOrderCommand = new(_ => PlaceOrder());
        }
        private void Search()
        {

        }
        private void PlaceOrder()
        {

        }
    }
}
