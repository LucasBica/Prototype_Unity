using LB.UIKit.Runtime.Components;

using UnityEngine;

namespace LB.UIKit.Runtime.Serializables {

    [CreateAssetMenu(fileName = nameof(UITransitionCombinatorViewController), menuName = UIKitConsts.PATH_ASSET_TRANSITION + nameof(UIViewController) + "/" + nameof(UITransitionCombinatorViewController), order = -1)]
    public class UITransitionCombinatorViewController : UITransitionCombinatorViewControllerBase<UIViewController> {

    }
}