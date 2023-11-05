using LB.Mvc.Runtime;

namespace LB.Authentication.Runtime.Mvc {

    public abstract class AuthentificationController : Controller<AuthenticationModel>, IAuthenticationController {

        protected IAuthenticationService authenticationService;

        public AuthentificationController(IAuthenticationService authenticationService) {
            this.authenticationService = authenticationService;
        }

        public void OnWillAppear(AuthenticationModel model) {
            model.animated = true;
            CallOnPropertyChanged(model);
        }

        public void OnClickGoogle(AuthenticationModel model) {
            if (model.isLoading) {
                return;
            }

        }

        public void OnClickApple(AuthenticationModel model) {
            if (model.isLoading) {
                return;
            }

        }

        public void OnClickMicrosoft(AuthenticationModel model) {
            if (model.isLoading) {
                return;
            }

        }
    }
}