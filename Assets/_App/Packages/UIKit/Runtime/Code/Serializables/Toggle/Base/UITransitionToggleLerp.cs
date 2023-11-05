using LB.UIKit.Runtime.Components;

using UnityEngine;

namespace LB.UIKit.Runtime.Serializables {

    public abstract class UITransitionToggleLerp<TView, TValue> : UITransitionToggleBase<TView> where TView : UIView {

        [SerializeField] protected ToggleStatesParams<TValue> toggleStatesParams = default;

        protected abstract bool IsOn(TView view);

        protected abstract ToggleParameter<TValue> GetParameter(TView view);

        protected abstract TValue GetFromValue(TView view);

        protected abstract void SetFromValue(TView view);

        protected abstract void SetCurrentValue(TView view, TValue value);

        protected abstract TValue Lerp(TView view, TValue from, TValue to, float time);

        protected virtual TValue GetTargetValue(TView view) {
            return GetParameter(view).value;
        }

        protected virtual float Evaluate(TView view, float time) {
            return GetParameter(view).curve.Evaluate(time);
        }
        public override void OnValidateView(TView view) {
            SetCurrentValue(view, IsOn(view) ? toggleStatesParams.isOn.value : toggleStatesParams.isOff.value);
        }

        public override void OnEnableView(TView view) {
            SetCurrentValue(view, IsOn(view) ? toggleStatesParams.isOn.value : toggleStatesParams.isOff.value);
        }

        public override void OnIsOn(TView toggle) {
            SetFromValue(toggle);
        }

        public override void OnIsOff(TView toggle) {
            SetFromValue(toggle);
        }

        public override void OnUpdateTransition(TView view, float time) {
            SetCurrentValue(view, Lerp(view, GetFromValue(view), GetTargetValue(view), Evaluate(view, time)));
        }

        public override void OnCompleteTransition(TView view) {
            // Nothing to do.
        }
    }
}