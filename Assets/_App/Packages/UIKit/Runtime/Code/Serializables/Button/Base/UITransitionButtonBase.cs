using LB.UIKit.Runtime.Components;

using UnityEngine;
using UnityEngine.EventSystems;


namespace LB.UIKit.Runtime.Serializables {

    public abstract class UITransitionButtonBase<T> : UITransitionScriptableObject<T>, IUIButtonEvents<T> where T : UIView {

        public abstract void OnInteractiveValueChanged(T button, bool interactive);

        public abstract void OnSelect(T button, BaseEventData eventData);

        public abstract void OnDeselect(T button, BaseEventData eventData);

        public abstract void OnPointerEnter(T button, PointerEventData eventData);

        public abstract void OnPointerDown(T button, PointerEventData eventData);

        public abstract void OnPointerUp(T button, PointerEventData eventData);

        public abstract void OnPointerExit(T button, PointerEventData eventData);

        public abstract void OnPointerClick(T button, PointerEventData eventData);

        [System.Serializable]
        public class ButtonStatesParams<TValue> {

            public ButtonParameter<TValue> normal = default;
            public ButtonParameter<TValue> selected = default;
            public ButtonParameter<TValue> highlighted = default;
            public ButtonParameter<TValue> pressed = default;
            public ButtonParameter<TValue> disabled = default;

            public ButtonParameter<TValue> GetByState(UIButton.States state) {
                return state switch {
                    UIButton.States.Normal => normal,
                    UIButton.States.Selected => selected,
                    UIButton.States.Highlighted => highlighted,
                    UIButton.States.Pressed => pressed,
                    UIButton.States.Disabled => disabled,
                    _ => null,
                };
            }
        }

        [System.Serializable]
        public class ButtonParameter<TValue> {
            public TValue value = default;
            public AnimationCurve curve = default;
        }
    }
}