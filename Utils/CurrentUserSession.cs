using RestaurantOrderingApp.Layers.EntityLayer;
using System.ComponentModel;

namespace RestaurantOrderingApp.Utils
{
    public class CurrentUserSession : INotifyPropertyChanged
    {
        private User? _currentUser;
        public User? CurrentUser
        {
            get => _currentUser;
            set
            {
                _currentUser = value;
                OnPropertyChanged(nameof(CurrentUser));
                OnPropertyChanged(nameof(IsEmployee));
            }
        }
        public bool IsEmployee => CurrentUser?.Role == "Employee";

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
