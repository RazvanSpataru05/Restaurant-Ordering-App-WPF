using System.Text.RegularExpressions;

namespace RestaurantOrderingApp.Utils
{
    public static class AuthenticationValidator
    {
        public static bool IsEmailValid(string email)
        {
            if (email == null) return false;
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }
        public static string IsPasswordValid(string password)
        {
            if (password == null) return "Password cannot be empty!";
            if (password.Length < 8) return "Password must contain at least 8 characters!";
            if (!password.Any(char.IsLower)) return "Password must contain at least a lowercase character!";
            if (!password.Any(char.IsUpper)) return "Passowrd must contain at least an uppercase character!";
            if (!password.Any(c => char.IsAsciiLetterOrDigit(c) == false)) return "Password must contain at least a special character!";
            return "Valid";
        }
        public static bool IsPhoneValid(string phone)
        {
            if (phone == null) return false;
            return Regex.IsMatch(phone, @"^\+?[0-9]{10,13}$");
        }
        public static bool IsDeliveryAddressValid(string deliveryAddress)
        {
            if (deliveryAddress == null) return false;
            return deliveryAddress.Length >= 15;
        }
    }
}
