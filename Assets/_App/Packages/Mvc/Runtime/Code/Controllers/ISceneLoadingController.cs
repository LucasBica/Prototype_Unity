namespace LB.Mvc.Runtime {

    public interface ISceneLoadingController : IController<SceneLoadingModel> {

        public void OnDidAppear(SceneLoadingModel model);
    }
}