using UnityEngine;
using UnityEngine.UI;

namespace LB.UIKit.Runtime.Components {

    public class ContentSizeController : UIView {

        [Header("Settings")]
        [SerializeField] private bool reBuildOnEnable = default;

        [Header("References")]
        [SerializeField] private RectTransform[] rectContentSizes = default;

        private void OnEnable() {
            if (reBuildOnEnable) {
                ForceReBuild();
            }
        }

        public void ForceReBuild() {
            for (int i = rectContentSizes.Length - 1; i >= 0; i--) {
                if (rectContentSizes[i] != null) {
                    LayoutRebuilder.ForceRebuildLayoutImmediate(rectContentSizes[i]);
                }
            }
        }
    }
}