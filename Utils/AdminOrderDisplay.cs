using System.Security.Permissions;

namespace RestaurantOrderingApp.Utils
{
    public class AdminOrderDisplay
    {
        public OrderDisplay OrderDisplay { get; set; }
        public string FirstName { get; set; } 
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string DeliveryAddress { get; set; }
        public string SelectedStatus { get; set; }
    }
}
