using RestaurantOrderingApp.Layers.EntityLayer;
using System.ComponentModel;


namespace RestaurantOrderingApp.Display
{
    public class AllergenDisplay : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private bool _isSelected;
        public Allergen Allergen { get; }
        public string Name => Allergen.Name;
        public int AllergenId => Allergen.AllergenId;
        public bool IsSelected
        {
            get => _isSelected;
            set { _isSelected = value; OnPropertyChanged(nameof(IsSelected)); }
        }

        public AllergenDisplay(Allergen allergen)
        {
            Allergen = allergen;
        }
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
