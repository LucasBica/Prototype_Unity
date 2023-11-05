using LB.UIKit.Runtime.Components;

using UnityEngine;
using UnityEngine.EventSystems;

namespace LB.UIKit.Runtime.Serializables {

    public abstract class UITransitionButtonLerp<TView, TValue> : UITransitionButtonBase<TView> where TView : UIView {

        [SerializeField] protected ButtonStatesParams<TValue> buttonStatesParams = default;

        protected abstract bool IsInteractive(TView view);

        protected abstract ButtonParameter<TValue> GetParameter(TView view);

        protected abstract TValue GetFromValue(TView view);

        protected abstract void SetFromValue(TView view);

        protected abstract void SetCurrentValue(TView view, TValue value);

        protected abstract TValue Lerp(TView view, TValue from, TValue to, float time);

        protected virtual TValue GetTargetValue(TView button) {
            return GetParameter(button).value;
        }

        protected virtual float Evaluate(TView button, float time) {
            return GetParameter(button).curve.Evaluate(time);
        }

        public override void OnValidateView(TView view) {
            SetCurrentValue(view, IsInteractive(view) ? buttonStatesParams.normal.value : buttonStatesParams.disabled.value);
        }

        public override void OnEnableView(TView view) {
            SetCurrentValue(view, IsInteractive(view) ? buttonStatesParams.normal.value : buttonStatesParams.disabled.value);
        }

        public override void OnInteractiveValueChanged(TView button, bool interactive) {
            SetFromValue(button);
        }

        public override void OnSelect(TView button, BaseEventData eventData) {
            SetFromValue(button);
        }

        public override void OnDeselect(TView button, BaseEventData eventData) {
            SetFromValue(button);
        }

        public override void OnPointerEnter(TView button, PointerEventData eventData) {
            SetFromValue(button);
        }

        public override void OnPointerDown(TView button, PointerEventData eventData) {
            SetFromValue(button);
        }

        public override void OnPointerUp(TView button, PointerEventData eventData) {
            SetFromValue(button);
        }

        public override void OnPointerExit(TView button, PointerEventData eventData) {
            SetFromValue(button);
        }

        public override void OnPointerClick(TView button, PointerEventData eventData) {
            SetFromValue(button);
        }

        public override void OnUpdateTransition(TView view, float time) {
            SetCurrentValue(view, Lerp(view, GetFromValue(view), GetTargetValue(view), Evaluate(view, time)));
        }

        public override void OnCompleteTransition(TView view) {
            // Nothing to do.
        }
    }
}