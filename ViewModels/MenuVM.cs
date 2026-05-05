using RestaurantOrderingApp.Layers.BusinessLogicLayer;
using RestaurantOrderingApp.Layers.EntityLayer;
using RestaurantOrderingApp.Utils;
using System.Collections.ObjectModel;

public enum AllergenFilter
{
    None,
    With,
    Without
};

namespace RestaurantOrderingApp.ViewModels
{
    public class MenuVM : BaseViewModel
    {
        private readonly ProductBLL _productBLL;
        private readonly CategoryBLL _categoryBLL;
        private readonly AllergenBLL _allergenBLL;
        private readonly MenuBLL _menuBLL;
        private readonly CartService _cartService;

        private AllergenFilter _allergenFilter;
        private string _searchText;
        private ObservableCollection<CategoryWithProducts> _fullMenu;
        private ObservableCollection<CategoryWithProducts> _filteredMenu;
        private ObservableCollection<Allergen> _allAlergens;
        private ObservableCollection<Allergen> _selectedAllergens;

        public AllergenFilter AllergenFilter
        {
            get => _allergenFilter;
            set
            {
                if (_allergenFilter != value)
                {
                    _allergenFilter = value;
                    OnPropertyChanged(nameof(AllergenFilter));
                }
            }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    OnPropertyChanged(nameof(SearchText));
                    Search();
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
        public ObservableCollection<Allergen> AllAllergens
        {
            get => _allAlergens;
            set
            {
                if (_allAlergens != value)
                {
                    _allAlergens = value;
                    OnPropertyChanged(nameof(AllAllergens));
                }
            }
        }
        public ObservableCollection<Allergen> SelectedAllergens
        {
            get => _selectedAllergens;
            set
            {
                if (_selectedAllergens != value)
                {
                    _selectedAllergens = value;
                    OnPropertyChanged(nameof(SelectedAllergens));
                }
            }
        }

        public RelayCommand SearchCommand { get; set; }
        public RelayCommand ToggleWithCommand { get; set; }
        public RelayCommand ToggleWithoutCommand { get; set; }
        public RelayCommand ToggleAllergenCommand { get; set; }
        public RelayCommand AddProductToCartCommand { get; set; }

        public MenuVM(ProductBLL productBLL, CategoryBLL categoryBLL, AllergenBLL allergenBLL, MenuBLL menuBLL, CartService cartService)
        {
            _productBLL = productBLL;
            _categoryBLL = categoryBLL;
            _allergenBLL = allergenBLL;
            _menuBLL = menuBLL;
            _cartService = cartService;

            FullMenu = [];
            FilteredMenu = [];
            AllAllergens = allergenBLL.GetAllAllergens();
            SelectedAllergens = [];

            var allProducts = _productBLL.GetAllProucts();
            var grouped = allProducts
                .GroupBy(p => p.CategoryName)
                .Select(g => new CategoryWithProducts(
                    category: g.Key,
                    products: new(g.ToList())));
            FullMenu = new(grouped);
            FilteredMenu = FullMenu;

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
            ToggleWithCommand = new(_ => ToggleWith());
            ToggleWithoutCommand = new(_ => ToggleWithout());
            ToggleAllergenCommand = new(param => ToggleAllergen(param as Allergen));
            AddProductToCartCommand = new(param => AddProductToCart(param as Product));
        }
        private void Search()
        {
            if (string.IsNullOrEmpty(SearchText)) return;

            SearchText = SearchText.Trim();
            var result = new ObservableCollection<CategoryWithProducts>();
            foreach (var category in _fullMenu)
            {
                var filteredProducts = category.Products
                    .Where(p => p.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
                if (SelectedAllergens.Any())
                {
                    if (AllergenFilter == AllergenFilter.With)
                    {
                        filteredProducts = filteredProducts
                            .Where(p => p.Allergens != null &&
                            SelectedAllergens.All(a => p.Allergens.Any(pa => pa.AllergenId == a.AllergenId)));
                    }
                    else if (AllergenFilter == AllergenFilter.Without)
                    {
                        filteredProducts = filteredProducts
                            .Where(p => p.Allergens == null ||
                            SelectedAllergens.Any(a => p.Allergens.Any(pa => pa.AllergenId == a.AllergenId)));
                    }
                }

                if (filteredProducts.Any())
                {
                    result.Add(new CategoryWithProducts(category.Category, new(filteredProducts)));
                }
            }
            FilteredMenu = result;
        }
        private void ToggleWith()
        {
            if (AllergenFilter == AllergenFilter.With)
                AllergenFilter = AllergenFilter.None;
            else
                AllergenFilter = AllergenFilter.With;
        }
        private void ToggleWithout()
        {
            if (AllergenFilter == AllergenFilter.Without)
                AllergenFilter = AllergenFilter.None;
            else
                AllergenFilter = AllergenFilter.Without;
        }
        private void ToggleAllergen(Allergen? allergen)
        {
            if (allergen == null) return;

            if (SelectedAllergens.Contains(allergen))
            {
                SelectedAllergens.Remove(allergen);
            }
            else
                SelectedAllergens.Add(allergen);
            Search();
        }
        private void AddProductToCart(Product? product)
        {
            if (product == null) return; 

            _cartService.AddCartItem(product);
        }
    }
}
