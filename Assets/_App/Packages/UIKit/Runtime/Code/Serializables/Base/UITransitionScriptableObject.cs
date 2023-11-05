using LB.UIKit.Runtime.Components;

namespace LB.UIKit.Runtime.Serializables {

    public abstract class UITransitionScriptableObject<T> : UITransitionView where T : UIView {

        public abstract void OnValidateView(T view);

        public abstract void OnEnableView(T view);

        public abstract void OnUpdateTransition(T view, float time);

        public abstract void OnCompleteTransition(T view);
    }
}