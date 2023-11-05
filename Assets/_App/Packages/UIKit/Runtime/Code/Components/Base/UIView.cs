using UnityEngine;

namespace LB.UIKit.Runtime.Components {

    public class UIView : MonoBehaviour {

        private RectTransform rectT;
        public RectTransform RectT {
            get {
                if (rectT == null) {
                    rectT = GetComponent<RectTransform>();
                }
                return rectT;
            }
        }
    }
}