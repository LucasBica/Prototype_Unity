using LB.Core.Runtime;
using LB.MessageSystem.Runtime;

using UnityEngine;

namespace LB.Mvc.Runtime.Components {

    public class MainCameraChangeNotifier : MonoBehaviour {

        [Header("References")]
        [SerializeField] private Camera mainCamera = default;

        private void Awake() {
            DIContainer.Get<IMessenger>().Send(MvcCallbacks.MainCameraChanged, mainCamera);
        }
    }
}