using RestaurantOrderingApp.Layers.EntityLayer;
using System.ComponentModel;

namespace RestaurantOrderingApp.Display
{
    public class ProductSelectionItem : INotifyPropertyChanged
    {
        private bool _isSelected;
        public Product Product { get; set; }
        public bool IsSelected
        {
            get => _isSelected;
            set { _isSelected = value; OnPropertyChanged(nameof(IsSelected)); }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
