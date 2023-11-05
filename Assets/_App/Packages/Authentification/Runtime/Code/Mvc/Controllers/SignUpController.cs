using System;

using LB.Authentication.Models;
using LB.MessageSystem.Runtime;
using LB.Mvc.Runtime;

namespace LB.Authentication.Runtime.Mvc {

    public class SignUpController : AuthentificationController, ISignUpController {

        private readonly IInfoController infoController;
        private readonly IMessenger messenger;

        private readonly Action<SignInResponse> actionSuccess;

        private readonly Func<SignUpRequest> funcSignUpRequest;
        private readonly Func<AuthenticationModel> funcAuthenticationModel;
        private readonly Func<AlertModel> funcAlertModel;

        public SignUpController(IAuthenticationService authenticationService, IInfoController infoController, IMessenger messenger, Action<SignInResponse> actionSuccess, Func<SignUpRequest> funcSignUpRequest, Func<AuthenticationModel> funcAuthenticationModel, Func<AlertModel> funcAlertModel) : base(authenticationService) {
            this.infoController = infoController;
            this.messenger = messenger;

            this.actionSuccess = actionSuccess;

            this.funcSignUpRequest = funcSignUpRequest;
            this.funcAuthenticationModel = funcAuthenticationModel;
            this.funcAlertModel = funcAlertModel;
        }

        public void OnClickSignUp(AuthenticationModel model) {
            if (model.isLoading) {
                return;
            }

            SignUpRequest signUpRequest = funcSignUpRequest.Invoke();
            signUpRequest.username = model.username;
            signUpRequest.email = model.email;
            signUpRequest.password = model.password;

            model.isLoading = true;
            CallOnPropertyChanged(model);

            authenticationService.SignUp(signUpRequest, (signInResponse) => {
                actionSuccess?.Invoke(signInResponse);
            },
            (httpError) => {
                model.isLoading = false;
                CallOnPropertyChanged(model);

                AlertModel alertModel = funcAlertModel.Invoke();
                alertModel.shouldAppear = true;
                alertModel.animated = true;
                alertModel.title = httpError.error.name;
                alertModel.description = httpError.error.message;
                alertModel.controllerDelegated = infoController;

                messenger.Send(MvcCallbacks.UpdateAlertView, alertModel);
            });
        }

        public void OnClickAppearSignIn(AuthenticationModel model) {
            if (model.isLoading) {
                return;
            }

            AuthenticationModel authenticationModel = funcAuthenticationModel.Invoke();
            authenticationModel.shouldDisappear = true;
            authenticationModel.animated = true;

            messenger.Send(AuthenticationCallbacks.UpdateSignUp, authenticationModel);
        }
    }
}