using RestaurantOrderingApp.Dialog_Service;
using RestaurantOrderingApp.Layers.BusinessLogicLayer;
using RestaurantOrderingApp.Layers.EntityLayer;
using RestaurantOrderingApp.Utils;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Windows;

namespace RestaurantOrderingApp.ViewModels
{
    public class AddProductVM : BaseViewModel
    {
        private readonly IDialogService _dialogService;
        private readonly ProductBLL _productBLL;
        private readonly CategoryBLL _categoryBLL;
        private readonly string? _defaultImagePath = Convert.ToString(ConfigurationManager.AppSettings["DefaultProductImage"]);

        private string _productName;
        private Category _selectedCategory;
        private string _portionQuantity;
        private string _totalQuantity;
        private decimal _price;

        public string ProductName
        {
            get => _productName;
            set { _productName = value; OnPropertyChanged(nameof(ProductName)); }
        }
        public Category SelectedCategory
        {
            get => _selectedCategory;
            set { _selectedCategory = value; OnPropertyChanged(nameof(SelectedCategory)); }
        }
        public string PortionQuantity
        {
            get => _portionQuantity;
            set { _portionQuantity = value; OnPropertyChanged(nameof(PortionQuantity)); }
        }
        public string TotalQuantity
        {
            get => _totalQuantity;
            set { _totalQuantity = value; OnPropertyChanged(nameof(TotalQuantity)); }
        }
        public decimal Price
        {
            get => _price;
            set {  _price = value; OnPropertyChanged(nameof(Price)); }
        }
        public ObservableCollection<Category> Categories { get; set; }

        public RelayCommand CancelCommand { get; set; }
        public RelayCommand ConfirmCommand { get; set; }
        public AddProductVM(IDialogService dialogService, ProductBLL productBLL, CategoryBLL categoryBLL)
        {
            _dialogService = dialogService;
            _productBLL = productBLL;
            _categoryBLL = categoryBLL;

            Categories = _categoryBLL.GetAllCategories();
            InitializeCommands();
        }
        private void InitializeCommands()
        {
            CancelCommand = new(_ => _dialogService.CloseAddProductWindow());
            ConfirmCommand = new(_ =>
            {
                if (!decimal.TryParse(TotalQuantity, out decimal totalQty) ||
                !decimal.TryParse(PortionQuantity, out decimal portionQty)) return;

                if (totalQty <= 0 || portionQty <= 0 || Price <= 0) return;

                if (SelectedCategory == null || string.IsNullOrEmpty(ProductName)) return;

                string unit = SelectedCategory.Name == "Bevande" ? "ml" : "g";
                string portionFormatted = $"{portionQty}{unit}";

                bool success = _productBLL.AddProduct(ProductName, SelectedCategory.CategoryId,
                    portionFormatted, totalQty, Price, _defaultImagePath!);

                if (!success)
                {
                    MessageBox.Show("A product with this name already exists!",
                        "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                _dialogService.CloseAddProductWindow();
            });
        }
    }
}
