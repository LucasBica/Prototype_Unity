using LB.Mvc.Runtime;

namespace LB.Authentication.Runtime.Mvc {

    public interface IAuthenticationController : IController<AuthenticationModel> {

        public void OnWillAppear(AuthenticationModel model);

        public void OnClickGoogle(AuthenticationModel model);

        public void OnClickApple(AuthenticationModel model);

        public void OnClickMicrosoft(AuthenticationModel model);
    }
}