using LB.UIKit.Runtime.Components;

using UnityEngine;

namespace LB.UIKit.Runtime.Serializables {

    [CreateAssetMenu(fileName = nameof(UITransitionViewController_Scale), menuName = UIKitConsts.PATH_ASSET_TRANSITION + nameof(UIViewController) + "/" + nameof(UITransitionViewController_Scale))]
    public class UITransitionViewController_Scale : UITransitionViewControllerLerp<UIViewController, Vector3> {

        protected override Vector3 Lerp(UIViewController view, Vector3 fromDisappeared, Vector3 toAppeared, float time) {
            return Vector3.LerpUnclamped(fromDisappeared, toAppeared, time);
        }

        protected override void SetCurrentValue(UIViewController viewController, Vector3 value) {
            viewController.RectContent.localScale = value;
        }
    }
}