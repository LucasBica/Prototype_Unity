namespace LB.Core.Runtime.Utilities {

    public interface IPasswordValidator {

        public bool IsValid(string password, out string message);
    }
}