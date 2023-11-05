namespace LB.Mvc.Runtime {

    public interface ISplashScreenController : IController<SplashScreenModel> {

        public void OnCompletedOne(SplashScreenModel model);

        public void OnCompletedAll(SplashScreenModel model);
    }
}