namespace LB.Core.Runtime.Utilities {

    public interface IEmailValidator {

        public bool IsValid(string email, out string message);
    }
}