using RestaurantOrderingApp.Dialog_Service;
using RestaurantOrderingApp.Display;
using RestaurantOrderingApp.Utils;

namespace RestaurantOrderingApp.ViewModels
{
    public class ProductDetailVM : BaseViewModel
    {
        private readonly IDialogService _dialogService;
        public string Name { get; set; }
        public string Price { get; set; }
        public string PortionQuantity { get; set; }
        public string ImagePath { get; set; }
        public string Description { get; set; }
        public RelayCommand CloseCommand { get; set; }

        public ProductDetailVM(IDialogService dialogService, ProductDisplay productDisplay)
        {
            _dialogService = dialogService;
            Name = productDisplay.Product.Name;
            Price = $"{productDisplay.Product.Price:F2} RON";
            PortionQuantity = productDisplay.Product.PortionQuantity;
            ImagePath = productDisplay.Product.ImagePath!;
            Description = productDisplay.Product.Description;

            CloseCommand = new(_ => _dialogService.CloseProductDetailWindow());
        }
    }
}
