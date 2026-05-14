using RestaurantOrderingApp.Layers.EntityLayer;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace RestaurantOrderingApp.Utils
{
    public class AdminOrderDisplay : INotifyPropertyChanged
    {
        private string _selectedStatus;

        public Order Order { get; set; }
        public ObservableCollection<OrderItem> Items { get; set; }
        public string FirstName { get; set; } 
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string DeliveryAddress { get; set; }
        public string SelectedStatus
        {
            get => _selectedStatus;
            set
            {
                if (_selectedStatus != value)
                {
                    _selectedStatus = value;
                    OnPropertyChanged(nameof(SelectedStatus));
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
