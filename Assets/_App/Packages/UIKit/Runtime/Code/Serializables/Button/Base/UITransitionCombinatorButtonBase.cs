using System;

using LB.UIKit.Runtime.Components;

using UnityEngine.EventSystems;

namespace LB.UIKit.Runtime.Serializables {

    public class UITransitionCombinatorButtonBase<T> : UITransitionCombinator<T, UITransitionButtonBase<T>>, IUIButtonEvents<T> where T : UIView {

        public virtual void OnInteractiveValueChanged(T button, bool interactive) {
            Array.ForEach(transitions, (transition) => { transition.OnInteractiveValueChanged(button, interactive); });
        }

        public virtual void OnSelect(T button, BaseEventData eventData) {
            Array.ForEach(transitions, (transition) => { transition.OnSelect(button, eventData); });
        }

        public virtual void OnDeselect(T button, BaseEventData eventData) {
            Array.ForEach(transitions, (transition) => { transition.OnDeselect(button, eventData); });
        }

        public virtual void OnPointerEnter(T button, PointerEventData eventData) {
            Array.ForEach(transitions, (transition) => { transition.OnPointerEnter(button, eventData); });
        }

        public virtual void OnPointerDown(T button, PointerEventData eventData) {
            Array.ForEach(transitions, (transition) => { transition.OnPointerDown(button, eventData); });
        }

        public virtual void OnPointerUp(T button, PointerEventData eventData) {
            Array.ForEach(transitions, (transition) => { transition.OnPointerUp(button, eventData); });
        }

        public virtual void OnPointerExit(T button, PointerEventData eventData) {
            Array.ForEach(transitions, (transition) => { transition.OnPointerExit(button, eventData); });
        }

        public virtual void OnPointerClick(T button, PointerEventData eventData) {
            Array.ForEach(transitions, (transition) => { transition.OnPointerClick(button, eventData); });
        }
    }
}