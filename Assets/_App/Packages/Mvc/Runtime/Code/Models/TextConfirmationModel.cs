namespace LB.Mvc.Runtime {

    public class TextConfirmationModel : ViewUpdaterModel {

        public string title;
        public string placeholder;
        public string inputText;
        public string textFirstButton;
        public string textSecondButton;
        public ITextConfirmationController controllerDelegated;
    }
}