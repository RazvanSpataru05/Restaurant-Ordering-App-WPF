using RestaurantOrderingApp.Layers.EntityLayer;
using RestaurantOrderingApp.ViewModels;
using System.Runtime.CompilerServices;

namespace RestaurantOrderingApp.Display
{
    public class ProductDisplay : BaseViewModel
    {
        private string _allergensText;
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
        public bool HasAllergens => Product?.Allergens != null && Product.Allergens.Count > 0;
        public ProductDisplay(Product product)
        {
            Product = product;
            SelectedQuantity = 1;
        }
        public string? CategoryName { get; set; }
        public string AllergensText
        {
            get => _allergensText;
            set => _allergensText = HasAllergens ? string.Join(" · ", Product.Allergens.Select(a => a.Name)) : string.Empty;
        }

}
}
