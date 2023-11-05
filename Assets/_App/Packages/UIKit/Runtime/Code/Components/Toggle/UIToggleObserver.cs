using LB.UIKit.Runtime.Serializables;

using UnityEngine;

namespace LB.UIKit.Runtime.Components {

    [RequireComponent(typeof(UIToggle))]
    public abstract class UIToggleObserver<T> : UIObserver<UIToggle> where T : UIView {

        [Header("Assets")]
        [SerializeField] protected UITransitionCombinatorToggleBase<T> transition = default;

        public override void OnValidate() {
            if (transition == null) {
                return;
            }

            transition.OnValidateView(GetThisInstance());
        }

        protected virtual void Awake() {
            Subscribe();
        }

        protected virtual void OnEnable() {
            if (transition == null) {
                return;
            }

            transition.OnEnableView(GetThisInstance());
        }

        protected abstract T GetThisInstance();

        protected void Subscribe() {
            Subject.OnIsOn += Subject_OnIsOn;
            Subject.OnIsOff += Subject_OnIsOff;
            Subject.OnUpdateTransition += Subject_OnUpdateTransition; ;
            Subject.OnCompleteTransition += Subject_OnCompleteTransition; ;
        }

        private void Subject_OnIsOn(UIToggle toggle) {
            if (transition == null) {
                return;
            }

            transition.OnIsOn(GetThisInstance());
        }

        private void Subject_OnIsOff(UIToggle toggle) {
            if (transition == null) {
                return;
            }

            transition.OnIsOff(GetThisInstance());
        }

        private void Subject_OnUpdateTransition(UIToggle toggle, float time) {
            if (transition == null) {
                return;
            }

            transition.OnUpdateTransition(GetThisInstance(), time);
        }

        private void Subject_OnCompleteTransition(UIToggle toggle, UITransitionCombinatorToggle transtionToggle) {
            if (transition == null) {
                return;
            }

            transition.OnCompleteTransition(GetThisInstance());
        }
    }
}