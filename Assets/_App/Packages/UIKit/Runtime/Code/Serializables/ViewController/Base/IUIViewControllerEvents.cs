using LB.UIKit.Runtime.Components;

namespace LB.UIKit.Runtime {

    public interface IUIViewControllerEvents<T> where T : UIView {

        public void OnWillAppear(T viewController);

        public void OnDidAppear(T viewController);

        public void OnWillDisappear(T viewController);

        public void OnDidDisappear(T viewController);

        public void OnUpdateTransition(T viewController, float time);
    }
}