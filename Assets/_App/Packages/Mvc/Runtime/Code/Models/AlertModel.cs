namespace LB.Mvc.Runtime {

    public class AlertModel : ViewUpdaterModel {

        public string title;
        public string description;
        public string textButton;
        public IAlertController controllerDelegated;
    }
}