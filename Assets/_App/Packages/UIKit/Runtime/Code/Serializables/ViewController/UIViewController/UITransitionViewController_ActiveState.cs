using LB.UIKit.Runtime.Components;

using UnityEngine;

namespace LB.UIKit.Runtime.Serializables {

    [CreateAssetMenu(fileName = nameof(UITransitionViewController_ActiveState), menuName = UIKitConsts.PATH_ASSET_TRANSITION + nameof(UIViewController) + "/" + nameof(UITransitionViewController_ActiveState))]
    public class UITransitionViewController_ActiveState : UITransitionViewControllerBase<UIViewController> {

        public override void OnValidateView(UIViewController viewController) {
            // Nothing to do.
        }

        public override void OnEnableView(UIViewController viewController) {
            // Nothing to do.
        }

        public override void OnWillAppear(UIViewController viewController) {
            viewController.gameObject.SetActive(true);
        }

        public override void OnDidAppear(UIViewController viewController) {
            // Nothing to do.
        }

        public override void OnWillDisappear(UIViewController viewController) {
            // Nothing to do.
        }

        public override void OnDidDisappear(UIViewController viewController) {
            viewController.gameObject.SetActive(false);
        }

        public override void OnUpdateTransition(UIViewController viewController, float time) {

        }

        public override void OnCompleteTransition(UIViewController viewController) {
            // Nothing to do.
        }
    }
}