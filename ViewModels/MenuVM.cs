using RestaurantOrderingApp.Dialog_Service;
using RestaurantOrderingApp.Display;
using RestaurantOrderingApp.Layers.BusinessLogicLayer;
using RestaurantOrderingApp.Layers.EntityLayer;
using RestaurantOrderingApp.Utils;
using System.Collections.ObjectModel;
using System.Data;
using System.Reflection.Metadata;

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
        private const int MENUS_PER_PAGE = 1;

        private record MenuBookPage(string? CategoryHeader, List<MenuDisplay> Menus);
        private List<MenuBookPage> _menuPages = [];

        private bool _isMenusMode;
        private record MenuPage(string? CategoryHeader, List<ProductDisplay> Products);
        private List<MenuPage> _pages = [];


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
        private ObservableCollection<MenuDisplay> _menus;
        private ObservableCollection<AllergenDisplay> _allAlergens;

        private int _currentPage;
        private int _leftPageNumber;
        private int _rightPageNumber;
        private string? _leftPageHeader;
        private string? _rightPageHeader;

        private ObservableCollection<ProductDisplay> _leftPageProducts;
        private ObservableCollection<ProductDisplay> _rightPageProducts;
        private bool _isFirstPage;
        private bool _canGoNext;
        private bool _canGoPrev;

        private ObservableCollection<MenuDisplay> _leftPageMenus = [];
        private ObservableCollection<MenuDisplay> _rightPageMenus = [];

        public bool IsMenusMode
        {
            get => _isMenusMode;
            set { _isMenusMode = value; OnPropertyChanged(nameof(IsMenusMode)); }
        }
        public ObservableCollection<MenuDisplay> LeftPageMenus
        {
            get => _leftPageMenus;
            set { _leftPageMenus = value; OnPropertyChanged(nameof(LeftPageMenus)); }
        }
        public ObservableCollection<MenuDisplay> RightPageMenus
        {
            get => _rightPageMenus;
            set { _rightPageMenus = value; OnPropertyChanged(nameof(RightPageMenus)); }
        }

        public AllergenFilter AllergenFilter
        {
            get => _allergenFilter;
            set { _allergenFilter = value; OnPropertyChanged(nameof(AllergenFilter)); }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText == value) return;
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
                Search();
            }
        }
        public ObservableCollection<CategoryWithProducts> FullMenu
        {
            get => _fullMenu;
            set { _fullMenu = value; OnPropertyChanged(nameof(FullMenu)); }
        }
        public ObservableCollection<CategoryWithProducts> FilteredMenu
        {
            get => _filteredMenu;
            set { _filteredMenu = value; OnPropertyChanged(nameof(FilteredMenu)); }
        }
        public ObservableCollection<MenuDisplay> Menus
        {
            get => _menus;
            set { _menus = value; OnPropertyChanged(nameof(Menus)); }
        }
        public ObservableCollection<Display.AllergenDisplay> AllAllergens
        {
            get => _allAlergens;
            set { _allAlergens = value; OnPropertyChanged(nameof(AllAllergens)); }
        }
        public int CurrentPage
        {
            get => _currentPage;
            set { _currentPage = value; OnPropertyChanged(nameof(CurrentPage)); }
        }
        public int LeftPageNumber
        {
            get => _leftPageNumber;
            set { _leftPageNumber = value; OnPropertyChanged(nameof(LeftPageNumber)); }
        }
        public int RightPageNumber
        {
            get => _rightPageNumber;
            set { _rightPageNumber = value; OnPropertyChanged(nameof(RightPageNumber)); }
        }
        public string? LeftPageHeader
        {
            get => _leftPageHeader;
            set { _leftPageHeader = value; OnPropertyChanged(nameof(LeftPageHeader)); }
        }
        public string? RightPageHeader
        {
            get => _rightPageHeader;
            set { _rightPageHeader = value; OnPropertyChanged(nameof(RightPageHeader)); }
        }

        public ObservableCollection<ProductDisplay> LeftPageProducts
        {
            get => _leftPageProducts;
            set { _leftPageProducts = value; OnPropertyChanged(nameof(LeftPageProducts)); }
        }
        public ObservableCollection<ProductDisplay> RightPageProducts
        {
            get => _rightPageProducts;
            set { _rightPageProducts = value; OnPropertyChanged(nameof(RightPageProducts)); }
        }
        public bool IsFirstPage
        {
            get => _isFirstPage;
            set { _isFirstPage = value; OnPropertyChanged(nameof(IsFirstPage)); }
        }
        public bool CanGoNext
        {
            get => _canGoNext;
            set { _canGoNext = value; OnPropertyChanged(nameof(CanGoNext)); }
        }
        public bool CanGoPrev
        {
            get => _canGoPrev;
            set { _canGoPrev = value; OnPropertyChanged(nameof(CanGoPrev)); }
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
        public RelayCommand NavigateToMenusCommand { get; set; }
        public RelayCommand IncreaseProductCommand { get; set; }
        public RelayCommand DecreaseProductCommand { get; set; }
        public RelayCommand AddToMenuCartCommand { get; set; }
        public RelayCommand IncreaseMenuCommand { get; set; }
        public RelayCommand DecreaseMenuCommand { get; set; }


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
            Menus = [];
            AllAllergens = new(_allergenBLL.GetAllAllergens().Select(a => new AllergenDisplay(a)));
            AllCategories = _categoryBLL.GetAllCategories();

            CurrentPage = 1;
            IsFirstPage = true;
            LeftPageNumber = (CurrentPage * 2) - 1;
            RightPageNumber = CurrentPage * 2;

            InitializeMenu();
            InitializeAllergens();
            InitializeCommands();
            GeneratePages();
        }
        private void Search()
        {
            var selected = AllAllergens.Where(a => a.IsSelected).Select(a => a.Allergen).ToList();
            var term = SearchText?.Trim() ?? string.Empty;
            bool allergenFilterActive = AllergenFilter != AllergenFilter.None && selected.Count > 0;

            if (term.Length == 0 && !allergenFilterActive)
            {
                FilteredMenu = new(FullMenu);
            }
            else
            {
                var result = new ObservableCollection<CategoryWithProducts>();
                foreach (var category in _fullMenu)
                {
                    IEnumerable<ProductDisplay> filtered = category.Products;

                    if (term.Length > 0)
                    {
                        filtered = filtered.Where(p =>
                            p.Product.Name.Contains(term, StringComparison.OrdinalIgnoreCase));
                    }

                    if (allergenFilterActive)
                    {
                        if (AllergenFilter == AllergenFilter.With)
                        {
                            filtered = filtered.Where(p =>
                                p.Product.Allergens != null &&
                                selected.All(a => p.Product.Allergens.Any(pa => pa.AllergenId == a.AllergenId)));
                        }
                        else
                        {
                            filtered = filtered.Where(p =>
                                p.Product.Allergens == null ||
                                selected.All(a => !p.Product.Allergens.Any(pa => pa.AllergenId == a.AllergenId)));
                        }
                    }

                    if (filtered.Any())
                        result.Add(new(category.Category, new(filtered)));
                }
                FilteredMenu = result;
            }

            if (term.Length > 0 || allergenFilterActive) IsMenusMode = false;
            CurrentPage = 1;
            GeneratePages();
        }
        private void ToggleWith()
        {
            if (AllergenFilter == AllergenFilter.With) AllergenFilter = AllergenFilter.None;
            else AllergenFilter = AllergenFilter.With;
            Search();
        }
        private void ToggleWithout()
        {
            if (AllergenFilter == AllergenFilter.Without) AllergenFilter = AllergenFilter.None;
            else AllergenFilter = AllergenFilter.Without;
            Search();
        }
        private void ToggleAllergen(AllergenDisplay? display)
        {
            if (display == null) return;

            display.IsSelected = !display.IsSelected;
            Search();
        }
        private void AddProductToCart(ProductDisplay? productDisplay)
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
        private void AddMenuToCart(MenuDisplay? menu)
        {
            if (menu == null) return;
            if (_currentUserSession.CurrentUser == null)
            {
                _dialogService.ShowGuestWarningWindow("You must be logged in to add menus to your cart!");
                return;
            }
            _cartService.AddCartItem(menu, menu.SelectedQuantity);
            menu.SelectedQuantity = 1;
        }
        private void IncreaseMenu(MenuDisplay? menu)
        {
            if (menu != null) menu.SelectedQuantity++;
        }
        private void DecreaseMenu(MenuDisplay? menu)
        {
            if (menu != null && menu.SelectedQuantity > 1) menu.SelectedQuantity--;
        }

        private bool CanAddProductToCart(ProductDisplay? productDisplay)
        {
            if (productDisplay == null) return false;

            return _cartService.GetAvailablePortions(productDisplay.Product) > 0;
        }
        private void NextPage()
        {
            if (!CanGoNext) return;
            CurrentPage += 1;
            RenderCurrentSpread();
        }
        private void PrevPage()
        {
            if (!CanGoPrev) return;
            CurrentPage -= 1;
            RenderCurrentSpread();
        }
        private void GeneratePages()
        {
            BuildPages();
            RenderCurrentSpread();
        }
        private void BuildPages()
        {
            if (IsMenusMode) BuildMenuPages();
            else BuildProductPages();
        }
        private void BuildProductPages()
        {
            _pages = [];
            foreach (var categoryGroup in FilteredMenu)
            {
                for (int i = 0; i < categoryGroup.Products.Count; i += PRODUCTS_PER_PAGE)
                {
                    var slice = categoryGroup.Products
                        .Skip(i).Take(PRODUCTS_PER_PAGE).ToList();
                    string? header = i == 0 ? categoryGroup.Category.Name : null;
                    _pages.Add(new(header, slice));
                }
            }
            TotalPages = _pages.Count;
        }
        private void BuildMenuPages()
        {
            _menuPages = [];
            var grouped = Menus
                .GroupBy(m => m.MenuEntity.CategoryId)
                .Select(g => new
                {
                    Category = AllCategories.FirstOrDefault(c => c.CategoryId == g.Key),
                    Menus = g.ToList()
                }).Where(g => g.Category != null);

            foreach (var group in grouped)
            {
                for (int i = 0; i < group.Menus.Count; i += MENUS_PER_PAGE)
                {
                    var slice = group.Menus.Skip(i).Take(MENUS_PER_PAGE).ToList();
                    string? header = i == 0 ? group.Category!.Name : null;
                    _menuPages.Add(new(header, slice));
                }
            }
            TotalPages = _menuPages.Count;
        }
        private void RenderCurrentSpread()
        {
            LeftPageProducts = [];
            RightPageProducts = [];
            LeftPageMenus = [];
            RightPageMenus = [];
            LeftPageHeader = null;
            RightPageHeader = null;

            LeftPageNumber = (CurrentPage * 2) - 1;
            RightPageNumber = CurrentPage * 2;
            IsFirstPage = CurrentPage == 1;
            CanGoPrev = CurrentPage > 1;

            if (IsMenusMode) RenderMenuSpread();
            else RenderProductSpread();
        }
        private void RenderProductSpread()
        {
            CanGoNext = _pages.Count >= 2 * CurrentPage;

            if (IsFirstPage)
            {
                if (_pages.Count > 0)
                {
                    RightPageProducts = new(_pages[0].Products);
                    RightPageHeader = _pages[0].CategoryHeader;
                }
                return;
            }

            int leftIndex = 2 * CurrentPage - 3;
            int rightIndex = 2 * CurrentPage - 2;

            if (leftIndex >= 0 && leftIndex < _pages.Count)
            {
                LeftPageProducts = new(_pages[leftIndex].Products);
                LeftPageHeader = _pages[leftIndex].CategoryHeader;
            }
            if (rightIndex >= 0 && rightIndex < _pages.Count)
            {
                RightPageProducts = new(_pages[rightIndex].Products);
                RightPageHeader = _pages[rightIndex].CategoryHeader;
            }
        }
        private void RenderMenuSpread()
        {
            int leftIndex = 2 * (CurrentPage - 1);
            int rightIndex = leftIndex + 1;
            CanGoNext = _menuPages.Count > 2 * CurrentPage;

            if (leftIndex >= 0 && leftIndex < _menuPages.Count)
            {
                LeftPageMenus = new(_menuPages[leftIndex].Menus);
                LeftPageHeader = _menuPages[leftIndex].CategoryHeader;
            }
            if (rightIndex >= 0 && rightIndex < _menuPages.Count)
            {
                RightPageMenus = new(_menuPages[rightIndex].Menus);
                RightPageHeader = _menuPages[rightIndex].CategoryHeader;
            }
        }
        private void NavigateToCategory(Category? category)
        {
            if (category == null) return;

            if (IsMenusMode)
            {
                IsMenusMode = false;
                BuildProductPages();
            }

            int pageIndex = _pages.FindIndex(p => p.CategoryHeader == category.Name);
            if (pageIndex < 0) return;

            CurrentPage = pageIndex == 0 ? 1 : ((pageIndex - 1) / 2) + 2;
            RenderCurrentSpread();
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
        private void InitializeMenu()
        {
            var allProducts = _productBLL.GetAllProucts();

            var grouped = allProducts
                .GroupBy(p => p.CategoryId)
                .Select(g =>
                {
                    var products = g.Select((p, i) => new ProductDisplay(p)
                    {
                        CategoryName = i == 0 ? AllCategories.First(c => c.CategoryId == g.Key).Name : null
                    });
                    return new CategoryWithProducts(
                        AllCategories.First(c => c.CategoryId == g.Key),
                        new(products));
                });

            FullMenu = new(grouped);
            FilteredMenu = FullMenu;
            Menus = _menuBLL.GetAllMenus();

            var menuItemsMap = _menuBLL.GetAllMenuItems();
            foreach (var menu in Menus)
            {
                if (menuItemsMap.TryGetValue(menu.MenuEntity.MenuId, out var items)) menu.Items = items;
            }
        }
        private void NavigateToMenus()
        {
            IsMenusMode = !IsMenusMode;
            CurrentPage = 1;
            GeneratePages();
        }
        private void InitializeAllergens()
        {
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
        }
        private void InitializeCommands()
        {
            SearchCommand = new(_ => Search());
            ToggleWithCommand = new(_ => ToggleWith());
            ToggleWithoutCommand = new(_ => ToggleWithout());
            ToggleAllergenCommand = new(param => ToggleAllergen(param as AllergenDisplay));
            AddProductToCartCommand = new(param => AddProductToCart(param as ProductDisplay),
                param => CanAddProductToCart(param as ProductDisplay));

            NextPageCommand = new(_ => NextPage());
            PrevPageCommand = new(_ => PrevPage());
            NavigateToCategoryCommand = new(param => NavigateToCategory(param as Category));
            IncreaseProductCommand = new(param => Increase(param as ProductDisplay),
                param => CanInrease(param as ProductDisplay));
            DecreaseProductCommand = new(param => Decrease(param as ProductDisplay),
                param => CanDecrease(param as ProductDisplay));
            NavigateToMenusCommand = new(_ => NavigateToMenus());

            AddToMenuCartCommand = new(param => AddMenuToCart(param as MenuDisplay));
            IncreaseMenuCommand = new(param => IncreaseMenu(param as MenuDisplay));
            DecreaseMenuCommand = new(param => DecreaseMenu(param as MenuDisplay));
        }
    }
}
