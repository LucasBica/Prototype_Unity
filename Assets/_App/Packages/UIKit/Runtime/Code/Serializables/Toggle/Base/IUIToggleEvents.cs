using LB.UIKit.Runtime.Components;

namespace LB.UIKit.Runtime {

    public interface IUIToggleEvents<T> where T : UIView {

        public void OnIsOn(T toggle);

        public void OnIsOff(T toggle);
    }
}