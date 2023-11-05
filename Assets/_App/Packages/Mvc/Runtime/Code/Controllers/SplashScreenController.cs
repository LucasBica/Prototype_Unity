using System;

namespace LB.Mvc.Runtime {

    public class SplashScreenController : Controller<SplashScreenModel>, ISplashScreenController {

        private readonly Action<SplashScreenModel> actionCompletedAll;

        public SplashScreenController(Action<SplashScreenModel> actionCompletedAll) {
            this.actionCompletedAll = actionCompletedAll;
        }

        public void OnCompletedOne(SplashScreenModel model) {
            model.splashScreenIndex++;
            CallOnPropertyChanged(model);
        }

        public void OnCompletedAll(SplashScreenModel model) {
            actionCompletedAll?.Invoke(model);
        }
    }
}