using System.Text.RegularExpressions;

namespace LB.Core.Runtime.Utilities {

    public class EmailValidator : IEmailValidator {

        public bool IsValid(string email, out string message) {
            Regex regexEmail = new(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,6}$", RegexOptions.None);
            bool isValid = !string.IsNullOrEmpty(email) && regexEmail.IsMatch(email);

            if (isValid) {
                message = string.Empty;
            } else {
                message = "An valid email is required.";
            }

            return isValid;
        }
    }
}