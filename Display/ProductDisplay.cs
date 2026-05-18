using RestaurantOrderingApp.Layers.EntityLayer;
using RestaurantOrderingApp.ViewModels;

namespace RestaurantOrderingApp.Display
{
    public class ProductDisplay : BaseViewModel
    {
        private string? _statusText;
        private int _selectedQuantity;
        public int SelectedQuantity
        {
            get => _selectedQuantity;
            set
            {
                if (_selectedQuantity != value)
                {
                    _selectedQuantity = value;
                    OnPropertyChanged(nameof(SelectedQuantity));
                }
            }
        }
        public Product Product { get; set; }
        public string? CategoryName { get; set; }
        public bool HasAllergens => Product?.Allergens != null && Product.Allergens.Count > 0;
        public string AllergensText => HasAllergens
            ? string.Join(" · ", Product.Allergens.Select(a => a.Name))
            : string.Empty;
        public string? StatusText
        {
            get => _statusText;
            set { _statusText = value; OnPropertyChanged(nameof(StatusText)); }
        }
        public ProductDisplay(Product product)
        {
            Product = product;
            SelectedQuantity = 1;
        }


}
}
