using RestaurantOrderingApp.Dialog_Service;
using RestaurantOrderingApp.Layers.BusinessLogicLayer;
using RestaurantOrderingApp.Layers.EntityLayer;
using RestaurantOrderingApp.Utils;
using System.Collections.ObjectModel;
using System.Data;
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
        private readonly CurrentUserSession _currentUserSession;
        private readonly IDialogService _dialogService;
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

        private int _currentPage;
        private int _leftPageNumber;
        private int _rightPageNumber;

        private ObservableCollection<ProductDisplay> _leftPageProducts;
        private ObservableCollection<ProductDisplay> _rightPageProducts;
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
        public ObservableCollection<ProductDisplay> LeftPageProducts
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
        public ObservableCollection<ProductDisplay> RightPageProducts
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
        public RelayCommand AddToCartCommand { get; set; }
        public RelayCommand NextPageCommand { get; set; }
        public RelayCommand PrevPageCommand { get; set; }
        public RelayCommand NavigateToCategoryCommand { get; set; }
        public RelayCommand IncreaseCommand { get; set; }
        public RelayCommand DecreaseCommand { get; set; }

        public MenuVM(ProductBLL productBLL, CategoryBLL categoryBLL, AllergenBLL allergenBLL, 
            MenuBLL menuBLL, CartService cartService, CurrentUserSession currentUserSession,
            IDialogService dialogService)
        {
            _productBLL = productBLL;
            _categoryBLL = categoryBLL;
            _allergenBLL = allergenBLL;
            _menuBLL = menuBLL;
            _cartService = cartService;
            _currentUserSession = currentUserSession;
            _dialogService = dialogService;

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
                    products: new(g.Select(p => new ProductDisplay(p)))));

            FullMenu = new(grouped);
            FilteredMenu = FullMenu;
            GeneratePages();

            var allergenMap = _allergenBLL.GetAllProductAllergens();
            foreach (var category in FullMenu)
            {
                foreach (var displayedProduct in category.Products)
                {
                    if (allergenMap.TryGetValue(displayedProduct.Product.ProductId, out ObservableCollection<Allergen>? value))
                    {
                        displayedProduct.Product.Allergens = value;
                    }
                }
            }

            SearchCommand = new(_ => Search());
            ToggleWithCommand = new(_ => ToggleWith());
            ToggleWithoutCommand = new(_ => ToggleWithout());
            ToggleAllergenCommand = new(param => ToggleAllergen(param as Allergen));
            AddToCartCommand = new(param => AddToCart(param as ProductDisplay), param => CanAddToCart(param as ProductDisplay));
            NextPageCommand = new(_ => NextPage());
            PrevPageCommand = new(_ => PrevPage());
            NavigateToCategoryCommand = new(param => NavigateToCategory(param as Category));
            IncreaseCommand = new(param => Increase(param as ProductDisplay), param => CanInrease(param as ProductDisplay));
            DecreaseCommand = new(param => Decrease(param as ProductDisplay), param => CanDecrease(param as ProductDisplay));
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
                    .Where(p => p.Product.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase));

                if (SelectedAllergens.Any())
                {
                    if (AllergenFilter == AllergenFilter.With)
                    {
                        filteredProducts = Enumerable.Where(filteredProducts, p => p.Product.Allergens != null &&
                            SelectedAllergens.All(a => p.Product.Allergens.Any(pa => pa.AllergenId == a.AllergenId)));
                    }
                    else if (AllergenFilter == AllergenFilter.Without)
                    {
                        filteredProducts = Enumerable.Where(filteredProducts, p => p.Product.Allergens == null ||
                            SelectedAllergens.Any(a => p.Product.Allergens.Any(pa => pa.AllergenId == a.AllergenId)));
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
        private void AddToCart(ProductDisplay? productDisplay)
        {
            if (productDisplay == null) return;
            if (_currentUserSession.CurrentUser == null)
            {
                _dialogService.ShowGuestWarningWindow("You must be logged in to add products to your cart!");
                return;
            }

            _cartService.AddCartItem(productDisplay.Product, productDisplay.SelectedQuantity);
            productDisplay.SelectedQuantity = 1;
        }
        private bool CanAddToCart(ProductDisplay? productDisplay)
        {
            if (productDisplay == null) return false;

            return _cartService.GetAvailablePortions(productDisplay.Product) > 0;
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
        private void Increase(ProductDisplay? productDisplay)
        {
            if (productDisplay == null) return;
            productDisplay.SelectedQuantity++;
        }
        private bool CanInrease(ProductDisplay? productDisplay)
        {
            if (productDisplay == null) return false;
            return productDisplay.SelectedQuantity < _cartService.GetAvailablePortions(productDisplay.Product);
        }
        private void Decrease(ProductDisplay? productDisplay) 
        {
            if (productDisplay == null) return;
            productDisplay.SelectedQuantity--;
        }
        private bool CanDecrease(ProductDisplay? productDisplay)
        {
            if (productDisplay == null) return false;
            return productDisplay.SelectedQuantity > 1;
        }
    }
}
