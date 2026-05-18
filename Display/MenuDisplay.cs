using RestaurantOrderingApp.Layers.EntityLayer;
using RestaurantOrderingApp.ViewModels;
using System.Collections.ObjectModel;

namespace RestaurantOrderingApp.Display
{
    public class MenuDisplay : BaseViewModel
    {
        private int _selectedQuantity = 1;
        private string? _statusText;
        public Menu MenuEntity { get; set; }
        public decimal CalculatedPrice { get; set; }
        public ObservableCollection<MenuItemDetail> Items { get; set; }
        
        public int SelectedQuantity
        {
            get => _selectedQuantity;
            set { _selectedQuantity = value; OnPropertyChanged(nameof(SelectedQuantity)); }
        }
        public string? StatusText
        {
            get => _statusText;
            set { _statusText = value; OnPropertyChanged(nameof(StatusText)); }
        }

        public decimal TotalPrice => CalculatedPrice * SelectedQuantity;
    }
}
