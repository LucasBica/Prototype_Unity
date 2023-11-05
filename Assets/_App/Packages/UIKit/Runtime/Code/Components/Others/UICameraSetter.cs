using UnityEngine;

namespace LB.UIKit.Runtime.Components {

    public class UICameraSetter : UIView {

        [Header("References")]
        [SerializeField] private Canvas canvas = default;

        private void Awake() {
            canvas.worldCamera = Camera.main;
        }
    }
}