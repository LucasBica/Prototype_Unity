namespace LB.Authentication.Runtime.Mvc {

    public interface ISignInController : IAuthenticationController {

        public void OnClickForgotPassword(AuthenticationModel model);

        public void OnClickSignIn(AuthenticationModel model);

        public void OnClickAppearSignUp(AuthenticationModel model);
    }
}