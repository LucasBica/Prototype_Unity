namespace LB.Mvc.Runtime {

    public class ChoiceConfirmationController : Controller<ChoiceConfirmationModel>, IChoiceConfirmationController {

        public void OnWillAppear(ChoiceConfirmationModel model) {
            model.controllerDelegated.OnPropertyChanged += ControllerDelegated_OnPropertyChanged;
            model.controllerDelegated.OnWillAppear(model);
        }

        public void OnWillDisappear(ChoiceConfirmationModel model) {
            model.controllerDelegated.OnWillDisappear(model);
            model.controllerDelegated.OnPropertyChanged -= ControllerDelegated_OnPropertyChanged;
        }

        public void OnClickButtonFirst(ChoiceConfirmationModel model) {
            model.controllerDelegated.OnClickButtonFirst(model);
        }

        public void OnClickButtonSecond(ChoiceConfirmationModel model) {
            model.controllerDelegated.OnClickButtonSecond(model);
        }

        private void ControllerDelegated_OnPropertyChanged(ChoiceConfirmationModel model) {
            CallOnPropertyChanged(model);
        }
    }
}