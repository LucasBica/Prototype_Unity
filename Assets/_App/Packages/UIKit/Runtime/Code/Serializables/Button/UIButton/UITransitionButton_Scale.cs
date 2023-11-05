using LB.UIKit.Runtime.Components;

using UnityEngine;

namespace LB.UIKit.Runtime.Serializables {

    [CreateAssetMenu(fileName = nameof(UITransitionButton_Scale), menuName = UIKitConsts.PATH_ASSET_TRANSITION + nameof(UIButton) + "/" + nameof(UITransitionButton_Scale))]
    public class UITransitionButton_Scale : UITransitionButtonLerp<UIButton, Vector2> {

        protected override bool IsInteractive(UIButton view) {
            return view.IsInteractive;
        }

        protected override ButtonParameter<Vector2> GetParameter(UIButton button) {
            return buttonStatesParams.GetByState(button.State);
        }

        protected override Vector2 GetFromValue(UIButton button) {
            object data = button.GetTransitionData(this);
            return data == null ? button.RectContent.localScale : (Vector2)data;
        }

        protected override void SetFromValue(UIButton button) {
            object data = (object)((Vector2)button.RectContent.localScale);
            button.SetTransitionData(this, data);
        }

        protected override void SetCurrentValue(UIButton button, Vector2 value) {
            button.RectContent.localScale = value;
        }

        protected override Vector2 Lerp(UIButton button, Vector2 from, Vector2 to, float time) {
            return Vector2.LerpUnclamped(from, to, time);
        }
    }
}