using System.Linq;
using System.Text.RegularExpressions;

namespace NoteBin
{
    public static partial class UserHelper
    {
        public static bool ValidateUsername(string name)
        {
            return !string.IsNullOrWhiteSpace(name) && name.Length <= Constants.UserNameLengthLimit && UsernameRegex().IsMatch(name);
        }

        public static bool ValidatePassword(string password)
        {
            return !string.IsNullOrWhiteSpace(password)
                && password.Length >= Constants.PasswordMinLength
                && password.Any(char.IsAsciiLetterLower)
                && password.Any(char.IsAsciiLetterUpper)
                && password.Any(char.IsAsciiDigit);
        }

        [GeneratedRegex("^[a-zA-Z0-9_]*$")]
        private static partial Regex UsernameRegex();
    }
}