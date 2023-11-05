using System;

using LB.UIKit.Runtime.Components;

namespace LB.UIKit.Runtime.Serializables {

    public class UITransitionCombinatorViewControllerBase<T> : UITransitionCombinator<T, UITransitionViewControllerBase<T>>, IUIViewControllerEvents<T> where T : UIView {

        public void OnDidAppear(T viewController) {
            Array.ForEach(transitions, (transition) => { transition.OnDidAppear(viewController); });
        }

        public void OnWillAppear(T viewController) {
            Array.ForEach(transitions, (transition) => { transition.OnWillAppear(viewController); });
        }

        public void OnDidDisappear(T viewController) {
            Array.ForEach(transitions, (transition) => { transition.OnDidDisappear(viewController); });
        }

        public void OnWillDisappear(T viewController) {
            Array.ForEach(transitions, (transition) => { transition.OnWillDisappear(viewController); });
        }
    }
}