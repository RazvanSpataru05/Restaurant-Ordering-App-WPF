using RestaurantOrderingApp.Layers.EntityLayer;

namespace RestaurantOrderingApp.Utils
{
    public class CurrentUserSession
    {
        public User? CurrentUser { get; set; }
        public bool IsEmployee => CurrentUser?.Role == "Employee";
    }
}
