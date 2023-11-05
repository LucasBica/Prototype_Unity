using LB.UIKit.Runtime.Components;

using UnityEngine;
using UnityEngine.EventSystems;

namespace LB.UIKit.Runtime.Serializables {

    [CreateAssetMenu(fileName = nameof(UITransitionButton_Debug), menuName = UIKitConsts.PATH_ASSET_TRANSITION + nameof(UIButton) + "/" + nameof(UITransitionButton_Debug))]
    public class UITransitionButton_Debug : UITransitionButtonBase<UIButton> {

        [SerializeField] private bool logOnUpdateTransition = default;

        private void Log(UIButton button, string toLog) {
            Debug.Log($"[{nameof(UITransitionButton_Debug)}] ({nameof(button)}: {button.name}) {toLog}");
        }

        public override void OnValidateView(UIButton button) {
            Log(button, nameof(OnValidateView));
        }

        public override void OnEnableView(UIButton button) {
            Log(button, nameof(OnEnableView));
        }

        public override void OnInteractiveValueChanged(UIButton button, bool interactive) {
            Log(button, $"{nameof(OnInteractiveValueChanged)}({nameof(interactive)}: {interactive})");
        }

        public override void OnSelect(UIButton button, BaseEventData eventData) {
            Log(button, $"{nameof(OnSelect)}({nameof(eventData)}: {eventData})");
        }

        public override void OnDeselect(UIButton button, BaseEventData eventData) {
            Log(button, $"{nameof(OnDeselect)}({nameof(eventData)}: {eventData})");
        }

        public override void OnPointerEnter(UIButton button, PointerEventData eventData) {
            Log(button, $"{nameof(OnPointerEnter)}({nameof(eventData)}: {eventData})");
        }

        public override void OnPointerDown(UIButton button, PointerEventData eventData) {
            Log(button, $"{nameof(OnPointerDown)}({nameof(eventData)}: {eventData})");
        }

        public override void OnPointerUp(UIButton button, PointerEventData eventData) {
            Log(button, $"{nameof(OnPointerUp)}({nameof(eventData)}: {eventData})");
        }

        public override void OnPointerExit(UIButton button, PointerEventData eventData) {
            Log(button, $"{nameof(OnPointerExit)}({nameof(eventData)}: {eventData})");
        }

        public override void OnPointerClick(UIButton button, PointerEventData eventData) {
            Log(button, $"{nameof(OnPointerClick)}({nameof(eventData)}: {eventData})");
        }

        public override void OnUpdateTransition(UIButton button, float time) {
            if (!logOnUpdateTransition) {
                return;
            }

            Log(button, $"{nameof(OnUpdateTransition)}({nameof(time)}: {time})");
        }

        public override void OnCompleteTransition(UIButton button) {
            Log(button, nameof(OnCompleteTransition));
        }
    }
}