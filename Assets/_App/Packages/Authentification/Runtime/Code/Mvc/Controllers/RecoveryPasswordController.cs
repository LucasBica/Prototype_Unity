using LB.Authentication.Models;
using LB.MessageSystem.Runtime;
using LB.Mvc.Runtime;

namespace LB.Authentication.Runtime.Mvc {

    public class RecoveryPasswordController : Controller<TextConfirmationModel>, IRecoveryPasswordController {

        private readonly IAuthenticationService authenticationService;
        private readonly IInfoController infoController;
        private readonly IMessenger messenger;

        public RecoveryPasswordController(IAuthenticationService authenticationService, IInfoController infoController, IMessenger messenger) {
            this.authenticationService = authenticationService;
            this.infoController = infoController;
            this.messenger = messenger;
        }

        public void OnWillAppear(TextConfirmationModel model) {
            model.title = "Recovery Password";
            model.placeholder = "example@email.com";
            model.textFirstButton = "DONE";
            model.textSecondButton = "CANCEL";
        }

        public void OnWillDisappear(TextConfirmationModel model) {

        }

        public void OnInputTextChangedValue(TextConfirmationModel model) {

        }

        public void OnClickButtonFirst(TextConfirmationModel model) {
            model.isLoading = true;
            CallOnPropertyChanged(model);

            authenticationService.RecoveryPassword(new RecoveryPasswordRequest { email = model.inputText },
                (x) => {
                    model.isLoading = false;
                    model.shouldDisappear = true;
                    CallOnPropertyChanged(model);
                    messenger.Send(MvcCallbacks.UpdateAlertView, new AlertModel {
                        shouldAppear = true,
                        animated = true,
                        title = "Email Sent",
                        description = "An email has been sent to the administrative email address on file. Check the inbox of the administrator's email account, and click the reset link provided.",
                        textButton = "GOT IT",
                        controllerDelegated = infoController,
                    });
                },
                (httpError) => {
                    model.isLoading = false;
                    model.shouldDisappear = true;
                    CallOnPropertyChanged(model);
                    messenger.Send(MvcCallbacks.UpdateAlertView, new AlertModel {
                        shouldAppear = true,
                        animated = true,
                        title = httpError.error.name,
                        description = httpError.error.message,
                        textButton = "GOT IT",
                        controllerDelegated = infoController,
                    });
                }
            );
        }

        public void OnClickButtonSecond(TextConfirmationModel model) {
            model.shouldDisappear = true;
            CallOnPropertyChanged(model);
        }
    }
}