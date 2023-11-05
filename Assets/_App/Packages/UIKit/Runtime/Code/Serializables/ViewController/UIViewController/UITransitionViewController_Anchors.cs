using LB.UIKit.Runtime.Components;

using UnityEngine;

using static LB.UIKit.Runtime.Serializables.UITransitionViewController_Anchors;

namespace LB.UIKit.Runtime.Serializables {

    [CreateAssetMenu(fileName = nameof(UITransitionViewController_Anchors), menuName = UIKitConsts.PATH_ASSET_TRANSITION + nameof(UIViewController) + "/" + nameof(UITransitionViewController_Anchors))]
    public class UITransitionViewController_Anchors : UITransitionViewControllerLerp<UIViewController, Anchors> {

        protected override Anchors Lerp(UIViewController view, Anchors fromDisappeared, Anchors toAppeared, float time) {
            return Anchors.Lerp(fromDisappeared, toAppeared, time);
        }

        protected override void SetCurrentValue(UIViewController viewController, Anchors value) {
            viewController.RectContent.anchorMin = value.min;
            viewController.RectContent.anchorMax = value.max;
        }

        [System.Serializable]
        public struct Anchors {

            public Vector2 max;
            public Vector2 min;

            public Anchors(Vector2 min, Vector2 max) {
                this.min = min;
                this.max = max;
            }

            public static Anchors Lerp(Anchors from, Anchors to, float time) {
                return new Anchors(Vector2.Lerp(from.min, to.min, time), Vector2.Lerp(from.max, to.max, time));
            }
        }
    }
}