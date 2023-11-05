using System.Text.RegularExpressions;

namespace LB.Core.Runtime.Utilities {

    public class UsernameValidator : IUsernameValidator {

        public bool IsValid(string username, out string message) {
            Regex regexUsername = new(@"^[A-Za-z0-9_]{2,25}$", RegexOptions.None);
            bool isValid = !string.IsNullOrEmpty(username) && regexUsername.IsMatch(username);

            if (isValid) {
                message = string.Empty;
            } else {
                message = "An valid username is required.";
            }

            return isValid;
        }
    }
}