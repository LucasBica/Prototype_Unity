using LB.UIKit.Runtime.Serializables;

using UnityEngine;
using UnityEngine.EventSystems;

namespace LB.UIKit.Runtime.Components {

    [RequireComponent(typeof(UIButton))]
    public abstract class UIButtonObserver<T> : UIObserver<UIButton> where T : UIView {

        [Header("Assets")]
        [SerializeField] protected UITransitionCombinatorButtonBase<T> transition = default;

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
            Subject.OnInteractiveValueChanged += Button_OnInteractiveValueChanged;
            Subject.OnSelectHandler += Button_OnSelectHandler;
            Subject.OnDeselectHandler += Button_OnDeselectHandler;
            Subject.OnEnter += Button_OnEnter;
            Subject.OnDown += Button_OnDown;
            Subject.OnUp += Button_OnUp;
            Subject.OnExit += Button_OnExit;
            Subject.OnClick += Button_OnClick;
            Subject.OnUpdateTransition += Button_OnUpdateTransition;
            Subject.OnCompleteTransition += Button_OnCompleteTransition;
        }

        protected virtual void Button_OnInteractiveValueChanged(UIButton button, bool interactive) {
            if (transition == null) {
                return;
            }

            transition.OnInteractiveValueChanged(GetThisInstance(), interactive);
        }

        protected virtual void Button_OnSelectHandler(UIButton button, BaseEventData eventData) {
            if (transition == null) {
                return;
            }

            transition.OnSelect(GetThisInstance(), eventData);
        }

        protected virtual void Button_OnDeselectHandler(UIButton button, BaseEventData eventData) {
            if (transition == null) {
                return;
            }

            transition.OnDeselect(GetThisInstance(), eventData);
        }

        protected virtual void Button_OnEnter(UIButton button, PointerEventData eventData) {
            if (transition == null) {
                return;
            }

            transition.OnPointerEnter(GetThisInstance(), eventData);
        }

        protected virtual void Button_OnDown(UIButton button, PointerEventData eventData) {
            if (transition == null) {
                return;
            }

            transition.OnPointerDown(GetThisInstance(), eventData);
        }

        protected virtual void Button_OnUp(UIButton button, PointerEventData eventData) {
            if (transition == null) {
                return;
            }

            transition.OnPointerUp(GetThisInstance(), eventData);
        }

        protected virtual void Button_OnExit(UIButton button, PointerEventData eventData) {
            if (transition == null) {
                return;
            }

            transition.OnPointerExit(GetThisInstance(), eventData);
        }

        protected virtual void Button_OnClick(UIButton button, PointerEventData eventData) {
            if (transition == null) {
                return;
            }

            transition.OnPointerClick(GetThisInstance(), eventData);
        }

        protected virtual void Button_OnUpdateTransition(UIButton button, float time) {
            if (transition == null) {
                return;
            }

            transition.OnUpdateTransition(GetThisInstance(), time);
        }

        protected virtual void Button_OnCompleteTransition(UIButton button, UITransitionCombinatorButton transitionButton) {
            if (transition == null) {
                return;
            }

            transition.OnCompleteTransition(GetThisInstance());
        }
    }
}