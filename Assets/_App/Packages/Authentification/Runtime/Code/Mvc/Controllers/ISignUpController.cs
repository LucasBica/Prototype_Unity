namespace LB.Authentication.Runtime.Mvc {

    public interface ISignUpController : IAuthenticationController {

        public void OnClickSignUp(AuthenticationModel model);

        public void OnClickAppearSignIn(AuthenticationModel model);
    }
}