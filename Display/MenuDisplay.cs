using RestaurantOrderingApp.Layers.EntityLayer;
using RestaurantOrderingApp.ViewModels;
using System.Collections.ObjectModel;
using System.Security.Policy;

namespace RestaurantOrderingApp.Display
{
    public class MenuDisplay : BaseViewModel
    {
        public Menu MenuEntity { get; set; }
        public decimal CalculatedPrice { get; set; }
        public ObservableCollection<MenuItemDetail> Items { get; set; }
        private int _selectedQuantity = 1;
        public int SelectedQuantity
        {
            get => _selectedQuantity;
            set { _selectedQuantity = value; OnPropertyChanged(nameof(SelectedQuantity)); }
        }

        public decimal TotalPrice => CalculatedPrice * SelectedQuantity;
    }
}
