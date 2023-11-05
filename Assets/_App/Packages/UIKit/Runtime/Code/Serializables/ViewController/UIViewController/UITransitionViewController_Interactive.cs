using LB.UIKit.Runtime.Components;

using UnityEngine;

namespace LB.UIKit.Runtime.Serializables {

    [CreateAssetMenu(fileName = nameof(UITransitionViewController_Interactive), menuName = UIKitConsts.PATH_ASSET_TRANSITION + nameof(UIViewController) + "/" + nameof(UITransitionViewController_Interactive))]
    public class UITransitionViewController_Interactive : UITransitionViewControllerBase<UIViewController> {

        public override void OnValidateView(UIViewController viewController) {
            // Nothing to do.
        }

        public override void OnEnableView(UIViewController viewController) {
            // Nothing to do.
        }

        public override void OnWillAppear(UIViewController viewController) {
            viewController.CanvasGroup.interactable = false;
            viewController.CanvasGroup.blocksRaycasts = true;
        }

        public override void OnDidAppear(UIViewController viewController) {
            viewController.CanvasGroup.interactable = true;
            viewController.CanvasGroup.blocksRaycasts = true;
        }

        public override void OnWillDisappear(UIViewController viewController) {
            viewController.CanvasGroup.interactable = false;
            viewController.CanvasGroup.blocksRaycasts = true;
        }

        public override void OnDidDisappear(UIViewController viewController) {
            viewController.CanvasGroup.interactable = false;
            viewController.CanvasGroup.blocksRaycasts = false;
        }

        public override void OnUpdateTransition(UIViewController viewController, float time) {
            // Nothing to do.
        }

        public override void OnCompleteTransition(UIViewController viewController) {
            // Nothing to do.
        }
    }
}