using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Lab.Application.Utils.RegExp
{
    public static class ValidationUtils
    {
        public static bool IsValidDate(string date)
        {
            if (string.IsNullOrEmpty(date))
                return false;

            string[] formats = { "yyyy-MM-dd", "dd/MM/yyyy", "MMMM d, yyyy" };
            return DateTime.TryParseExact(date, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
        }

        public static bool IsValidEmail(string email)
        {
            const string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, emailPattern);
        }

        public static bool IsValidPhoneNumber(string phoneNumber)
        {
            const string phonePattern = @"^\+?[1-9]\d{9,14}$";
            return Regex.IsMatch(phoneNumber, phonePattern);
        }
    }
}
