namespace LB.Mvc.Runtime {

    public class ChoiceConfirmationModel : ViewUpdaterModel {

        public string title;
        public string description;
        public string textFirstButton;
        public string textSecondButton;
        public IChoiceConfirmationController controllerDelegated;
    }
}