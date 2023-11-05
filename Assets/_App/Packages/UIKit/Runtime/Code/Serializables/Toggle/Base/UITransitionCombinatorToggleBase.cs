using System;

using LB.UIKit.Runtime.Components;

namespace LB.UIKit.Runtime.Serializables {

    public class UITransitionCombinatorToggleBase<T> : UITransitionCombinator<T, UITransitionToggleBase<T>>, IUIToggleEvents<T> where T : UIView {

        public void OnIsOn(T toggle) {
            Array.ForEach(transitions, (transition) => { transition.OnIsOn(toggle); });
        }

        public void OnIsOff(T toggle) {
            Array.ForEach(transitions, (transition) => { transition.OnIsOff(toggle); });
        }
    }
}