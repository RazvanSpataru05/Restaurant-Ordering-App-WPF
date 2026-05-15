using RestaurantOrderingApp.Layers.EntityLayer;
using System.Collections.ObjectModel;

namespace RestaurantOrderingApp.Display
{
    public class OrderDisplay
    {
        public Order Order { get; set; }
        public ObservableCollection<OrderItem> Items { get; set; }
    }
}
