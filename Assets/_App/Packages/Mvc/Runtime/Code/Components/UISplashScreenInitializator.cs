using LB.Core.Runtime;
using LB.MessageSystem.Runtime;

using UnityEngine;

namespace LB.Mvc.Runtime {

    public class UISplashScreenInitializator : MonoBehaviour {

        protected virtual void Start() {
            DIContainer.Get<IMessenger>().Send(MvcCallbacks.UpdateSplashScreen, new SplashScreenModel {
                shouldAppear = true,
            });
        }
    }
}