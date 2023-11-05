using UnityEngine;

namespace LB.TweenSystem.Runtime {

    public class TweenInitializer : MonoBehaviour {
        
        protected virtual void OnEnable() {
            Tween.Initialize();
        }
    }
}