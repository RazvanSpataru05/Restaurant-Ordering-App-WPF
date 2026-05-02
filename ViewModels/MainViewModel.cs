
namespace RestaurantOrderingApp.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private BaseViewModel _currentViewModel;
        public BaseViewModel CurrentViewModel
        {
            get { return _currentViewModel; }
            set
            {
                if (_currentViewModel != value)
                {
                    _currentViewModel = value;
                    OnPropertyChanged(nameof(CurrentViewModel));
                }
            }
        }
        public void NavigateTo(BaseViewModel vm)
        {
            CurrentViewModel = vm;
        }
    }
}
