using System;

using LB.Authentication.Runtime;
using LB.Mvc.Runtime;

namespace LB.Authentication.Runtime.Mvc {

    public class SignOutController : Controller<ChoiceConfirmationModel>, ISignOutController {

        private readonly IAuthenticationService authenticationService;
        private readonly Action<ChoiceConfirmationModel> actionSignOut;

        public SignOutController(IAuthenticationService authenticationService, Action<ChoiceConfirmationModel> actionSignOut) {
            this.authenticationService = authenticationService;
            this.actionSignOut = actionSignOut;
        }

        public void OnWillAppear(ChoiceConfirmationModel model) {
            model.title = "Logout";
            model.description = "Are you sure want to logout?";
            model.textFirstButton = "LOGOUT";
            model.textSecondButton = "CANCEL";
        }

        public void OnWillDisappear(ChoiceConfirmationModel model) {

        }

        public void OnClickButtonFirst(ChoiceConfirmationModel model) {
            authenticationService.SignOut();
            actionSignOut?.Invoke(model);
        }

        public void OnClickButtonSecond(ChoiceConfirmationModel model) {
            model.shouldDisappear = true;
            CallOnPropertyChanged(model);
        }
    }
}