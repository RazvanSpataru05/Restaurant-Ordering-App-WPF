using RestaurantOrderingApp.Layers.BusinessLogicLayer;
using RestaurantOrderingApp.Layers.EntityLayer;
using RestaurantOrderingApp.Utils;
using System.Collections.ObjectModel;
using System.Reflection;

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
        private readonly int PRODUCTS_PER_PAGE = 3;
        private readonly ProductBLL _productBLL;
        private readonly CategoryBLL _categoryBLL;
        private readonly AllergenBLL _allergenBLL;
        private readonly MenuBLL _menuBLL;
        private readonly CartService _cartService;
        private readonly int _totalPages;
        private readonly ObservableCollection<Category> _allCategories;

        private AllergenFilter _allergenFilter;
        private string _searchText;
        private ObservableCollection<CategoryWithProducts> _fullMenu;
        private ObservableCollection<CategoryWithProducts> _filteredMenu;
        private ObservableCollection<Allergen> _allAlergens;
        private ObservableCollection<Allergen> _selectedAllergens;

        private int _currentPage;
        private int _leftPageNumber;
        private int _rightPageNumber;

        private ObservableCollection<Product> _leftPageProducts;
        private ObservableCollection<Product> _rightPageProducts;
        private bool _isFirstPage;
        private bool _canGoNext;
        private bool _canGoPrev;

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
        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                if (_currentPage != value)
                {
                    _currentPage = value;
                    OnPropertyChanged(nameof(CurrentPage));
                }
            }
        }
        public int LeftPageNumber
        {
            get => _leftPageNumber;
            set
            {
                if (_leftPageNumber != value)
                {
                    _leftPageNumber = value;
                    OnPropertyChanged(nameof(LeftPageNumber));
                }
            }
        }
        public int RightPageNumber
        {
            get => _rightPageNumber;
            set
            {
                if (_rightPageNumber != value)
                {
                    _rightPageNumber = value;
                    OnPropertyChanged(nameof(RightPageNumber));
                }
            }
        }
        public ObservableCollection<Product> LeftPageProducts
        {
            get => _leftPageProducts;
            set
            {
                if (_leftPageProducts != value)
                {
                    _leftPageProducts = value;
                    OnPropertyChanged(nameof(LeftPageProducts));
                }
            }
        }
        public ObservableCollection<Product> RightPageProducts
        {
            get => _rightPageProducts;
            set
            {
                if (_rightPageProducts != value)
                {
                    _rightPageProducts = value;
                    OnPropertyChanged(nameof(RightPageProducts));
                }
            }
        }
        public bool IsFirstPage
        {
            get => _isFirstPage;
            set
            {
                if (_isFirstPage != value)
                {
                    _isFirstPage = value;
                    OnPropertyChanged(nameof(IsFirstPage));
                }
            }
        }
        public bool CanGoNext
        {
            get => _canGoNext;
            set
            {
                if (_canGoNext != value)
                {
                    _canGoNext = value;
                    OnPropertyChanged(nameof(CanGoNext));
                }
            }
        }
        public bool CanGoPrev
        {
            get => _canGoPrev;
            set
            {
                if (_canGoPrev != value)
                {
                    _canGoPrev = value;
                    OnPropertyChanged(nameof(CanGoPrev));
                }
            }
        }

        public int TotalPages { get; set; }
        public ObservableCollection<Category> AllCategories { get; set; }

        public RelayCommand SearchCommand { get; set; }
        public RelayCommand ToggleWithCommand { get; set; }
        public RelayCommand ToggleWithoutCommand { get; set; }
        public RelayCommand ToggleAllergenCommand { get; set; }
        public RelayCommand AddProductToCartCommand { get; set; }
        public RelayCommand NextPageCommand { get; set; }
        public RelayCommand PrevPageCommand { get; set; }
        public RelayCommand NavigateToCategoryCommand { get; set; }

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
            AllCategories = _categoryBLL.GetAllCategories();
            SelectedAllergens = [];

            CurrentPage = 1;
            IsFirstPage = true;
            LeftPageNumber = (CurrentPage * 2) - 1;
            RightPageNumber = CurrentPage * 2;

            var allProducts = _productBLL.GetAllProucts();
            TotalPages = (int)Math.Ceiling(allProducts.Count / (double)(PRODUCTS_PER_PAGE) * 2);

            var grouped = allProducts
                .GroupBy(p => p.CategoryId)
                .Select(g => new CategoryWithProducts(
                    category: AllCategories.First(c => c.CategoryId == g.Key),
                    products: new(g.ToList())));

            FullMenu = new(grouped);
            FilteredMenu = FullMenu;
            GeneratePages();

            var allergenMap = _allergenBLL.GetAllProductAllergens();
            foreach (var category in FullMenu)
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
            NextPageCommand = new(_ => NextPage());
            PrevPageCommand = new(_ => PrevPage());
            NavigateToCategoryCommand = new(param => NavigateToCategory(param as Category));
        }
        private void Search()
        {
            if (string.IsNullOrEmpty(SearchText))
            {
                FilteredMenu = new(FullMenu);
                return;
            }

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
            GeneratePages();
        }
        private void ToggleWith()
        {
            if (AllergenFilter == AllergenFilter.With)
                AllergenFilter = AllergenFilter.None;
            else
                AllergenFilter = AllergenFilter.With;
            Search();
        }
        private void ToggleWithout()
        {
            if (AllergenFilter == AllergenFilter.Without)
                AllergenFilter = AllergenFilter.None;
            else
                AllergenFilter = AllergenFilter.Without;
            Search();
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
        private void NextPage()
        {
            if (!CanGoNext) return;

            CurrentPage += 1;
            GeneratePages();
        }
        private void PrevPage()
        {
            if (!CanGoPrev) return;

            CurrentPage -= 1;
            GeneratePages();
        }
        private void GeneratePages()
        {
            LeftPageProducts = [];
            RightPageProducts = [];
            LeftPageNumber = (CurrentPage * 2) - 1;
            RightPageNumber = CurrentPage * 2;
            CanGoNext = RightPageNumber < TotalPages / 2 + TotalPages % 2;
            CanGoPrev = CurrentPage > 1;
            IsFirstPage = CurrentPage == 1;

            int startIndex = (LeftPageNumber - 1) * PRODUCTS_PER_PAGE;
            var allProducts = FilteredMenu.SelectMany(c => c.Products).ToList();

            if (!IsFirstPage)
            {
                for (int index = startIndex; index < startIndex + PRODUCTS_PER_PAGE && index < allProducts.Count; index++)
                {
                    LeftPageProducts.Add(allProducts[index]);
                }
                startIndex += PRODUCTS_PER_PAGE;
            }    

            for (int index = startIndex; index < startIndex + PRODUCTS_PER_PAGE && index < allProducts.Count; index++)
            {
                RightPageProducts.Add(allProducts[index]);
            }
        }
        private void NavigateToCategory(Category? category)
        {
            if (category == null) return;

            var allProducts = FullMenu.SelectMany(p => p.Products).ToList();
            var firstProduct = FilteredMenu.FirstOrDefault(c => c.Category.CategoryId == category.CategoryId)?.Products.FirstOrDefault();
            if (firstProduct == null) return;

            int productIndex = allProducts.IndexOf(firstProduct);
            CurrentPage = productIndex / (PRODUCTS_PER_PAGE * 2) + 1;
            GeneratePages();
        }
    }
}
