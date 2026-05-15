using RestaurantOrderingApp.Layers.EntityLayer;
using RestaurantOrderingApp.ViewModels;

namespace RestaurantOrderingApp.Display
{
    public class ProductDisplay : BaseViewModel
    {
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
        public ProductDisplay(Product product)
        {
            Product = product;
            SelectedQuantity = 1;
        }
    }
}
