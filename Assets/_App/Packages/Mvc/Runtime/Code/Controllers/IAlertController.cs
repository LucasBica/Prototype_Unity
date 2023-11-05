namespace LB.Mvc.Runtime {

    public interface IAlertController : IController<AlertModel> {

        public void OnWillAppear(AlertModel model);

        public void OnWillDisappear(AlertModel model);

        public void OnClickButton(AlertModel model);
    }
}