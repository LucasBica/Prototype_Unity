using LB.UIKit.Runtime.Components;

using UnityEngine;

namespace LB.UIKit.Runtime.Serializables {

    public abstract class UITransitionToggleBase<T> : UITransitionScriptableObject<T>, IUIToggleEvents<T> where T : UIView {

        public abstract void OnIsOff(T toggle);

        public abstract void OnIsOn(T toggle);

        [System.Serializable]
        public class ToggleStatesParams<TValue> {

            public ToggleParameter<TValue> isOn = default;
            public ToggleParameter<TValue> isOff = default;

            public ToggleParameter<TValue> GetByState(UIToggle.States state) {
                return state switch {
                    UIToggle.States.IsOn => isOn,
                    UIToggle.States.IsOff => isOff,
                    _ => null,
                };
            }
        }

        [System.Serializable]
        public class ToggleParameter<TValue> {
            public TValue value = default;
            public AnimationCurve curve = default;
        }
    }
}