namespace LB.Mvc.Runtime {

    public interface IChoiceConfirmationController : IController<ChoiceConfirmationModel> {

        public void OnWillAppear(ChoiceConfirmationModel model);

        public void OnWillDisappear(ChoiceConfirmationModel model);

        public void OnClickButtonFirst(ChoiceConfirmationModel model);

        public void OnClickButtonSecond(ChoiceConfirmationModel model);
    }
}