using RestaurantOrderingApp.Layers.EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantOrderingApp.Utils
{
    public class CurrentUserSession
    {
        public User? CurrentUser { get; set; }
        public bool IsAuthenticated => CurrentUser != null;
        public bool IsEmployee => CurrentUser?.Role == "Employee";
        public bool IsGuest => CurrentUser == null;
    }
}
