using UnityEngine;

namespace LB.UIKit.Runtime.Serializables {

    public abstract class UITransitionView : ScriptableObject {

        // This class is useful only to indicate a reference of a UITransitionScriptableObject<TView> with any TView type.
        protected virtual void OnValidate() { }
    }
}