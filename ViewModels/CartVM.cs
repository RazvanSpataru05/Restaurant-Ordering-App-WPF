using RestaurantOrderingApp.Utils;

namespace RestaurantOrderingApp.ViewModels
{
    public class CartVM : BaseViewModel
    {
        public RelayCommand PlaceOrderCommand { get; set; }
        public CartVM()
        {
            PlaceOrderCommand = new(_ => PlaceOrder());
        }
        private void PlaceOrder()
        {

        }
    }
}
