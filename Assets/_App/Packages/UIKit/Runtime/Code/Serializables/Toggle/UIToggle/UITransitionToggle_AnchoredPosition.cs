using LB.UIKit.Runtime.Components;

using UnityEngine;

namespace LB.UIKit.Runtime.Serializables {

    [CreateAssetMenu(fileName = nameof(UITransitionToggle_AnchoredPosition), menuName = UIKitConsts.PATH_ASSET_TRANSITION + nameof(UIToggle) + "/" + nameof(UITransitionToggle_AnchoredPosition))]
    public class UITransitionToggle_AnchoredPosition : UITransitionToggleLerp<UIToggle, Vector2> {

        protected override bool IsOn(UIToggle view) {
            return view.IsOn;
        }

        protected override ToggleParameter<Vector2> GetParameter(UIToggle view) {
            return toggleStatesParams.GetByState(view.IsOn ? UIToggle.States.IsOn : UIToggle.States.IsOff);
        }

        protected override Vector2 GetFromValue(UIToggle view) {
            object data = view.GetTransitionData(this);
            return data == null ? view.RectContent.anchoredPosition : (Vector2)data;
        }

        protected override void SetFromValue(UIToggle view) {
            object data = view.RectContent.anchoredPosition;
            view.SetTransitionData(this, data);
        }

        protected override void SetCurrentValue(UIToggle view, Vector2 value) {
            view.RectContent.anchoredPosition = value;
        }

        protected override Vector2 Lerp(UIToggle view, Vector2 from, Vector2 to, float time) {
            return Vector2.LerpUnclamped(from, to, time);
        }
    }
}