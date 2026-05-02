using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RestaurantOrderingApp.Utils
{
    public static class AuthenticationValidator
    {
        public static bool IsEmailValid(string email)
        {
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }
        public static bool IsPasswordValid(string password)
        {
            return password.Length >= 8 &&
                password.Any(char.IsUpper) &&
                password.Any(char.IsLower) &&
                password.Any(c => char.IsAsciiLetterOrDigit(c) == false);
        }
        public static bool IsPhoneValid(string phone)
        {
            return Regex.IsMatch(phone, @"^\+?[0-9]{10,13}$");
        }
        public static bool IsDeliveryAddressValid(string deliveryAddress)
        {
            return deliveryAddress.Length >= 15;
        }
    }
}
