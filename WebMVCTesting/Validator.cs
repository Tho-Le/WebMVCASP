using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebMVCDemo.Validator
{
    public static class Validator
    {
        public static bool ValidatePassword(string password)
        {
            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasMinimum8Chars = new Regex(@".{8,}");

            var isValidated = hasUpperChar.IsMatch(password) && hasNumber.IsMatch(password)
                && hasMinimum8Chars.IsMatch(password);
            return isValidated;
        }
    }
}
