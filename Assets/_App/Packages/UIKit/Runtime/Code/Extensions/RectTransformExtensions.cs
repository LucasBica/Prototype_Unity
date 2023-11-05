using UnityEngine;

namespace LB.UIKit.Runtime.Extensions {

    public static class RectTransformExtensions {

        public static RectTransform GetRectCanvasRoot(this RectTransform rectTransform) {
            Canvas canvasRoot = rectTransform.GetCanvasRoot();
            if (canvasRoot == null) {
                return null;
            }

            return canvasRoot.GetComponent<RectTransform>();
        }

        public static Canvas GetCanvasRoot(this RectTransform rectTransform) {
            if (rectTransform == null) {
                return null;
            }

            Canvas canvas = rectTransform.GetComponent<Canvas>();
            if (canvas != null && canvas.rootCanvas != null) {
                return canvas.rootCanvas;
            }

            canvas = rectTransform.GetComponentInParent<Canvas>();
            if (canvas != null && canvas.rootCanvas != null) {
                return canvas.rootCanvas != null ? canvas.rootCanvas : canvas;
            }

            return null;
        }
    }
}