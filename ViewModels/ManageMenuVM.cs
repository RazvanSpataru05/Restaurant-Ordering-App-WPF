using RestaurantOrderingApp.Dialog_Service;
using RestaurantOrderingApp.Layers.BusinessLogicLayer;
using RestaurantOrderingApp.Utils;
using System.Collections.ObjectModel;

namespace RestaurantOrderingApp.ViewModels
{
    public class ManageMenuVM : BaseViewModel
    {
        private readonly IDialogService _dialogService;
        private readonly MenuBLL _menuBLL;
        private readonly ProductBLL _productBLL;

        private Layers.EntityLayer.Menu _selectedMenu;
        private decimal _calculatedPrice;

        public Layers.EntityLayer.Menu SelectedMenu
        {
            get => _selectedMenu;
            set
            { 
                _selectedMenu = value;
                OnPropertyChanged(nameof(SelectedMenu));

                var menuProducts = _menuBLL.GetMenuProducts(SelectedMenu.MenuId);
                var allProducts = _productBLL.GetAllProucts();

                Products = new(
                    allProducts.Select(p => new ProductSelectionItem
                    {
                        Product = p,
                        IsSelected = menuProducts.Any(mp => mp.ProductId == p.ProductId)
                    }));
            }
        }
        public decimal CalculatedPrice
        {
            get => _calculatedPrice;
            set { _calculatedPrice = value; OnPropertyChanged(nameof(CalculatedPrice)); }
        }

        public ObservableCollection<Layers.EntityLayer.Menu> Menus { get; set; }
        public ObservableCollection<ProductSelectionItem> Products { get; set; }

        public RelayCommand ConfirmCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }
        public ManageMenuVM(IDialogService dialogService, MenuBLL menuBLL, ProductBLL productBLL)
        {
            _dialogService = dialogService;
            _menuBLL = menuBLL;
            _productBLL = productBLL;

            Menus = _menuBLL.GetAllMenus();
            InitializeCommands();
        }
        private void InitializeCommands()
        {
            CancelCommand = new(_ => _dialogService.CloseManageMenuWindow());
            ConfirmCommand = new(_ =>
            {
                if (SelectedMenu == null) return;

                var selectedProducts = Products.Where(p => p.IsSelected).ToList();
                if (!selectedProducts.Any()) return;

                decimal newPrice = selectedProducts.Sum(p => p.Product.Price);

                _dialogService.CloseManageMenuWindow();
            });
        }
    }
}
