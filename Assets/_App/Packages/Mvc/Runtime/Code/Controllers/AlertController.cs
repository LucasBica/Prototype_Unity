namespace LB.Mvc.Runtime {

    public class AlertController : Controller<AlertModel>, IAlertController {

        public void OnWillAppear(AlertModel model) {
            model.controllerDelegated.OnPropertyChanged += ControllerDelegated_OnPropertyChanged;
            model.controllerDelegated.OnWillAppear(model);
        }

        public void OnWillDisappear(AlertModel model) {
            model.controllerDelegated.OnWillDisappear(model);
            model.controllerDelegated.OnPropertyChanged -= ControllerDelegated_OnPropertyChanged;
        }

        public void OnClickButton(AlertModel model) {
            model.controllerDelegated.OnClickButton(model);
        }

        private void ControllerDelegated_OnPropertyChanged(AlertModel model) {
            CallOnPropertyChanged(model);
        }
    }
}