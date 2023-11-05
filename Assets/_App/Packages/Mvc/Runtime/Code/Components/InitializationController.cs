using LB.Core.Runtime;
using LB.MessageSystem.Runtime;

using UnityEngine;

namespace LB.Mvc.Runtime.Components {

    public class InitializationController : MonoBehaviour {

        [Header("Settings")]
        [SerializeField] protected string sceneToLoad = default;

        [Header("References")]
        [SerializeField] protected Context context = default;

        protected virtual void Awake() {
            context.OnInitialized += Context_OnInitialized;
        }

        protected virtual void Context_OnInitialized() {
            DIContainer.Get<IMessenger>().Send(MvcCallbacks.UpdateLoadingView, new SceneLoadingModel {
                shouldAppear = true,
                animated = false,
                isVisible = false,
                sceneToLoad = sceneToLoad,
                shouldTryLoadNext = true,
            });
        }
    }
}