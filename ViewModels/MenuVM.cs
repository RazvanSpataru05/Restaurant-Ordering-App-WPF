using RestaurantOrderingApp.Layers.BusinessLogicLayer;
using RestaurantOrderingApp.Layers.EntityLayer;
using RestaurantOrderingApp.Utils;
using System.Collections.ObjectModel;

namespace RestaurantOrderingApp.ViewModels
{
    public class MenuVM : BaseViewModel
    {
        private readonly ProductBLL _productBLL;
        private readonly CategoryBLL _categoryBLL;
        private readonly AllergenBLL _allergenBLL;

        private string _searchText;
        private ObservableCollection<CategoryWithProducts> _fullMenu;
        private ObservableCollection<CategoryWithProducts> _filteredMenu;
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    OnPropertyChanged(nameof(SearchText));
                }
            }
        }
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
        public RelayCommand SearchCommand { get; set; }
        public MenuVM(ProductBLL productBLL, CategoryBLL categoryBLL, AllergenBLL allergenBLL)
        {
            _productBLL = productBLL;
            _categoryBLL = categoryBLL;
            _allergenBLL = allergenBLL;
            FullMenu = [];
            FilteredMenu = [];

            var allProducts = _productBLL.GetAllProucts();
            var grouped = allProducts
                .GroupBy(p => p.CategoryName)
                .Select(g => new CategoryWithProducts(
                    category: g.Key,
                    products: new(g.ToList())));
            FullMenu = new(grouped);

            var allergenMap = _allergenBLL.GetAllProductAllergens();
            foreach (var category in _fullMenu)
            {
                foreach (var product in category.Products)
                {
                    if (allergenMap.TryGetValue(product.ProductId, out ObservableCollection<Allergen>? value))
                    {
                        product.Allergens = value;
                    }
                }
            }

            SearchCommand = new(_ => Search());
        }
        private void Search()
        {

        }
    }
}
