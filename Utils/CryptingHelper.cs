using System.Security.Cryptography;
using System.Text;

namespace RestaurantOrderingApp.Utils
{
    public static class CryptingHelper
    {
        public static string HashPassword(string password)
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(password);
            byte[] hash = SHA256.HashData(inputBytes);
            return Convert.ToHexString(hash);
        }
    }
}
