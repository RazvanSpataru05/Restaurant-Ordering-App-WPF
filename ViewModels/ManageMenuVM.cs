using RestaurantOrderingApp.Dialog_Service;
using RestaurantOrderingApp.Display;
using RestaurantOrderingApp.Layers.BusinessLogicLayer;
using RestaurantOrderingApp.Layers.EntityLayer;
using RestaurantOrderingApp.Utils;
using System.Collections.ObjectModel;
using System.Configuration;

namespace RestaurantOrderingApp.ViewModels
{
    public class ManageMenuVM : BaseViewModel
    {
        private readonly decimal _menuDiscount = Convert.ToDecimal(ConfigurationManager.AppSettings["MenuDiscount"]);
        private readonly IDialogService _dialogService;
        private readonly MenuBLL _menuBLL;
        private readonly ProductBLL _productBLL;
        private readonly ObservableCollection<Product> _allProducts;

        private MenuDisplay _selectedMenu;
        private decimal _calculatedPrice;
        private ObservableCollection<ProductSelectionItem> _products;

        public MenuDisplay SelectedMenu
        {
            get => _selectedMenu;
            set
            { 
                _selectedMenu = value;
                OnPropertyChanged(nameof(SelectedMenu));

                var menuProducts = _menuBLL.GetMenuProducts(SelectedMenu.MenuEntity.MenuId);

                Products = new(
                    _allProducts.Select(p => new ProductSelectionItem
                    {
                        Product = p,
                        IsSelected = menuProducts.Any(mp => mp.ProductId == p.ProductId)
                    }));
                UpdateCalculatedPrice();
            }
        }
        public decimal CalculatedPrice
        {
            get => _calculatedPrice;
            set { _calculatedPrice = value; OnPropertyChanged(nameof(CalculatedPrice)); }
        }

        public ObservableCollection<MenuDisplay> Menus { get; set; }
        public ObservableCollection<ProductSelectionItem> Products
        {
            get => _products;
            set { _products = value; OnPropertyChanged(nameof(Products)); }
        }

        public RelayCommand ConfirmCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }
        public RelayCommand ToggleSelectionCommand { get; set; }
        public ManageMenuVM(IDialogService dialogService, MenuBLL menuBLL, ProductBLL productBLL)
        {
            _dialogService = dialogService;
            _menuBLL = menuBLL;
            _productBLL = productBLL;

            Menus = _menuBLL.GetAllMenus();
            _allProducts = _productBLL.GetAllProucts();
            InitializeCommands();
        }
        private void InitializeCommands()
        {
            CancelCommand = new(_ => _dialogService.CloseManageMenuWindow());
            ConfirmCommand = new(_ =>
            {
                if (SelectedMenu == null) return;

                var selectedProducts = Products.Where(p => p.IsSelected).ToList();

                _menuBLL.ClearMenuProducts(SelectedMenu.MenuEntity.MenuId);
                foreach (var selectedProduct in selectedProducts)
                {
                    _menuBLL.AddMenuProduct(SelectedMenu.MenuEntity.MenuId, selectedProduct.Product.ProductId, selectedProduct.Product.PortionQuantity);
                }
                _dialogService.CloseManageMenuWindow();
            }, _ => CanConfirm());

            ToggleSelectionCommand = new(param =>
            {
                if (param is ProductSelectionItem item)
                {
                    item.IsSelected = !item.IsSelected;
                    UpdateCalculatedPrice();
                }
            });
        }
        private bool CanConfirm()
        {
            return SelectedMenu != null && Products != null && Products.Any(p => p.IsSelected);
        }
        private void UpdateCalculatedPrice()
        {
            CalculatedPrice = Math.Floor(Products.Where(p => p.IsSelected).ToList().Sum(p => p.Product.Price) * (1 - _menuDiscount));
        }
    }
}
