using LB.UIKit.Runtime.Components;

using UnityEngine;

namespace LB.UIKit.Runtime.Serializables {

    public abstract class UITransitionViewControllerLerp<TView, TValue> : UITransitionViewControllerBase<TView> where TView : UIView {

        [SerializeField] protected TValue disappearedValue;
        [SerializeField] protected TValue appearedValue;

        protected abstract void SetCurrentValue(TView viewController, TValue value);

        protected abstract TValue Lerp(TView viewController, TValue fromDisappeared, TValue toAppeared, float time);

        public override void OnValidateView(TView viewController) {
            // Nothing to do.
        }

        public override void OnEnableView(TView viewController) {
            // Nothing to do.
        }

        public override void OnWillAppear(TView viewController) {
            // Nothing to do.
        }

        public override void OnDidAppear(TView viewController) {
            // Nothing to do.
        }

        public override void OnWillDisappear(TView viewController) {
            // Nothing to do.
        }

        public override void OnDidDisappear(TView viewController) {
            // Nothing to do.
        }

        public override void OnUpdateTransition(TView viewController, float time) {
            SetCurrentValue(viewController, Lerp(viewController, disappearedValue, appearedValue, Curve.Evaluate(time)));
        }

        public override void OnCompleteTransition(TView viewController) {
            // Nothing to do.
        }
    }
}