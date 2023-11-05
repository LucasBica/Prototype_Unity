namespace LB.Mvc.Runtime {

    public interface ITextConfirmationController : IController<TextConfirmationModel> {

        public void OnWillAppear(TextConfirmationModel model);

        public void OnWillDisappear(TextConfirmationModel model);

        public void OnInputTextChangedValue(TextConfirmationModel model);

        public void OnClickButtonFirst(TextConfirmationModel model);

        public void OnClickButtonSecond(TextConfirmationModel model);
    }
}