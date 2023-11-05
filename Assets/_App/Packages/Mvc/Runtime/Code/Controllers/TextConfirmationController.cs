namespace LB.Mvc.Runtime {

    public class TextConfirmationController : Controller<TextConfirmationModel>, ITextConfirmationController {

        public void OnWillAppear(TextConfirmationModel model) {
            model.controllerDelegated.OnPropertyChanged += ControllerDelegated_OnPropertyChanged;
            model.controllerDelegated.OnWillAppear(model);
        }

        public void OnWillDisappear(TextConfirmationModel model) {
            model.controllerDelegated.OnWillDisappear(model);
            model.controllerDelegated.OnPropertyChanged -= ControllerDelegated_OnPropertyChanged;
        }

        public void OnInputTextChangedValue(TextConfirmationModel model) {
            model.controllerDelegated.OnInputTextChangedValue(model);
        }

        public void OnClickButtonFirst(TextConfirmationModel model) {
            model.controllerDelegated.OnClickButtonFirst(model);
        }

        public void OnClickButtonSecond(TextConfirmationModel model) {
            model.controllerDelegated.OnClickButtonSecond(model);
        }

        private void ControllerDelegated_OnPropertyChanged(TextConfirmationModel model) {
            CallOnPropertyChanged(model);
        }
    }
}