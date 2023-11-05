namespace LB.Core.Runtime.Utilities {

    public interface IUsernameValidator {

        public bool IsValid(string username, out string message);
    }
}