using System.Text.RegularExpressions;

namespace LB.Core.Runtime.Utilities {

    public class PasswordValidator : IPasswordValidator {

        public bool IsValid(string password, out string message) {
            Regex regexPassword = new(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&#])[A-Za-z\d@$!%*?&#]{8,64}$", RegexOptions.None);
            bool isValid = !string.IsNullOrEmpty(password) && regexPassword.IsMatch(password);

            if (isValid) {
                message = string.Empty;
            } else {
                message = "An valid password is required.";
            }

            return isValid;
        }
    }
}