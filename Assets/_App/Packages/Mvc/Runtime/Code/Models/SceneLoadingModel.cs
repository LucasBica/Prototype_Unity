using LB.Core.Runtime.Utilities;

namespace LB.Mvc.Runtime {

    public class SceneLoadingModel : ViewUpdaterModel {

        public string sceneToLoad;
        public bool shouldTryLoadNext;
        public bool isVisible;

        public override string ToString() {
            return StaticUtilities.ToString(this);
        }
    }
}