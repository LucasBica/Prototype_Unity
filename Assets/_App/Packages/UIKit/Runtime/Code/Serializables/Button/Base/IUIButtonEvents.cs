using LB.UIKit.Runtime.Components;

using UnityEngine.EventSystems;

namespace LB.UIKit.Runtime {

    public interface IUIButtonEvents<T> where T : UIView {

        public void OnInteractiveValueChanged(T button, bool interactive);

        public void OnSelect(T button, BaseEventData eventData);

        public void OnDeselect(T button, BaseEventData eventData);

        public void OnPointerEnter(T button, PointerEventData eventData);

        public void OnPointerDown(T button, PointerEventData eventData);

        public void OnPointerUp(T button, PointerEventData eventData);

        public void OnPointerExit(T button, PointerEventData eventData);

        public void OnPointerClick(T button, PointerEventData eventData);
    }
}