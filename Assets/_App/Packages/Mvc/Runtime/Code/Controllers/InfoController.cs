namespace LB.Mvc.Runtime {

    public class InfoController : Controller<AlertModel>, IInfoController {

        public void OnWillAppear(AlertModel model) {
            model.textButton = "GOT IT";
        }

        public void OnWillDisappear(AlertModel model) {
            
        }

        public void OnClickButton(AlertModel model) {
            model.shouldDisappear = true;
            CallOnPropertyChanged(model);
        }
    }
}