using System.Text.RegularExpressions;

namespace LB.Core.Runtime.Utilities {

    public class VersionValidator : IVersionValidator {

        public bool IsValid(string version) {
            Regex regexVersion = new(@"^[a-z][0-9][.]{1,}$", RegexOptions.None);
            return !string.IsNullOrEmpty(version) && regexVersion.IsMatch(version);
        }
    }
}