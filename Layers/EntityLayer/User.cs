using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantOrderingApp.Layers.EntityLayer
{
    public class User
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string DeliveryAddress { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; }
    }
}
