using LB.UIKit.Runtime.Components;

using UnityEngine;

namespace LB.UIKit.Runtime.Serializables {

    public abstract class UITransitionViewControllerBase<T> : UITransitionScriptableObject<T>, IUIViewControllerEvents<T> where T : UIView {

        [SerializeField] protected AnimationCurve curve = default;

        public AnimationCurve Curve => curve;

        public abstract void OnDidAppear(T viewController);

        public abstract void OnWillAppear(T viewController);

        public abstract void OnDidDisappear(T viewController);

        public abstract void OnWillDisappear(T viewController);
    }
}