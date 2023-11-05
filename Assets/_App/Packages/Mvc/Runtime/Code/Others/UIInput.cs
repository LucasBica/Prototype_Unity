using System;

using LB.UIKit.Runtime.Components;

using UnityEngine;

namespace LB.Mvc.Runtime.Components {

    public class UIInput : UIView {

        public static event Action OnBack;

        protected virtual void Update() {
            if (Application.platform == RuntimePlatform.Android && Input.GetKeyUp(KeyCode.Escape)) {
                OnBack?.Invoke();
            }
        }
    }
}