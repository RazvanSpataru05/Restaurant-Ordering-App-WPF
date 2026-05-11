using RestaurantOrderingApp.Layers.DataAccessLayer;
using RestaurantOrderingApp.Utils;
using RestaurantOrderingApp.Layers.EntityLayer;

namespace RestaurantOrderingApp.Layers.BusinessLogicLayer
{
    public class UserBLL
    {
        private readonly UserDAL _userDAL = new();
        public User? AuthenticateUser(string email, string password)
        {
            return _userDAL.AuthenticateUser(email, CryptingHelper.HashPassword(password));
        }
        public bool RegisterUser(User user)
        {
            return _userDAL.RegisterUser(user);
        }
    }
}
