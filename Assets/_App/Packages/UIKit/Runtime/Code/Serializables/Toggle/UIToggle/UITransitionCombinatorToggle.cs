using LB.UIKit.Runtime.Components;

using UnityEngine;

namespace LB.UIKit.Runtime.Serializables {

    [CreateAssetMenu(fileName = nameof(UITransitionCombinatorToggle), menuName = UIKitConsts.PATH_ASSET_TRANSITION + nameof(UIToggle) + "/" + nameof(UITransitionCombinatorToggle), order = -1)]
    public class UITransitionCombinatorToggle : UITransitionCombinatorToggleBase<UIToggle> {
        
    }
}