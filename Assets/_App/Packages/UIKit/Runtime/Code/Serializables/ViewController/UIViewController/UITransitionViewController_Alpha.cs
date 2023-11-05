using LB.UIKit.Runtime.Components;

using UnityEngine;

namespace LB.UIKit.Runtime.Serializables {

    [CreateAssetMenu(fileName = nameof(UITransitionViewController_Alpha), menuName = UIKitConsts.PATH_ASSET_TRANSITION + nameof(UIViewController) + "/" + nameof(UITransitionViewController_Alpha))]
    public class UITransitionViewController_Alpha : UITransitionViewControllerLerp<UIViewController, float> {

        protected override float Lerp(UIViewController view, float fromDisappeared, float toAppeared, float time) {
            return Mathf.LerpUnclamped(fromDisappeared, toAppeared, time);
        }

        protected override void SetCurrentValue(UIViewController viewController, float value) {
            viewController.CanvasGroup.alpha = value;
        }
    }
}