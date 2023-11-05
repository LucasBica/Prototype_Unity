using System;

using LB.Authentication.Models;
using LB.MessageSystem.Runtime;
using LB.Mvc.Runtime;

namespace LB.Authentication.Runtime.Mvc {

    public class SignInController : AuthentificationController, ISignInController {

        private readonly IRecoveryPasswordController recoveryPasswordController;
        private readonly IInfoController infoController;
        private readonly IMessenger messenger;

        private readonly Action<SignInResponse> actionSuccess;

        private readonly Func<SignInRequest> funcSignInRequest;
        private readonly Func<AuthenticationModel> funcAuthenticationModel;
        private readonly Func<AlertModel> funcAlertModel;
        private readonly Func<TextConfirmationModel> funcTextConfirmationModel;

        public SignInController(IAuthenticationService authenticationService, IRecoveryPasswordController recoveryPasswordController, IInfoController infoController, IMessenger messenger, Action<SignInResponse> actionSuccess, Func<SignInRequest> funcSignInRequest, Func<AuthenticationModel> funcAuthenticationModel, Func<AlertModel> funcAlertModel, Func<TextConfirmationModel> funcTextConfirmationModel) : base(authenticationService) {
            this.recoveryPasswordController = recoveryPasswordController;
            this.infoController = infoController;
            this.messenger = messenger;

            this.actionSuccess = actionSuccess;

            this.funcSignInRequest = funcSignInRequest;
            this.funcAuthenticationModel = funcAuthenticationModel;
            this.funcAlertModel = funcAlertModel;
            this.funcTextConfirmationModel = funcTextConfirmationModel;
        }

        public void OnClickForgotPassword(AuthenticationModel model) {
            if (model.isLoading) {
                return;
            }

            TextConfirmationModel textConfirmationModel = funcTextConfirmationModel.Invoke();
            textConfirmationModel.shouldAppear = true;
            textConfirmationModel.animated = true;
            textConfirmationModel.controllerDelegated = recoveryPasswordController;

            messenger.Send(MvcCallbacks.UpdateTextConfirmationView, textConfirmationModel);
        }

        public void OnClickSignIn(AuthenticationModel model) {
            if (model.isLoading) {
                return;
            }

            SignInRequest signInRequest = funcSignInRequest.Invoke();
            signInRequest.identifier = model.email;
            signInRequest.password = model.password;

            model.isLoading = true;
            CallOnPropertyChanged(model);

            authenticationService.SignIn(signInRequest, (signInResponse) => {
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

        public void OnClickAppearSignUp(AuthenticationModel model) {
            if (model.isLoading) {
                return;
            }

            AuthenticationModel authenticationModel = funcAuthenticationModel.Invoke();
            authenticationModel.shouldAppear = true;
            authenticationModel.animated = true;

            messenger.Send(AuthenticationCallbacks.UpdateSignUp, authenticationModel);
        }
    }
}