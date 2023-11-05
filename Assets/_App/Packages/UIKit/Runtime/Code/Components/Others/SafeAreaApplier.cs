using System;

using LB.UIKit.Runtime.Extensions;

using UnityEngine;

namespace LB.UIKit.Runtime.Components {

    public class SafeAreaApplier : UIView {

        [Header("Settings")]
        [SerializeField] private VerticalOperationTypes operationTop = VerticalOperationTypes.SubtractTopMargin;
        [SerializeField] private VerticalOperationTypes operationBottom = VerticalOperationTypes.AddBottomMargin;
        [SerializeField] private HorizontalOperationTypes operationLeft = HorizontalOperationTypes.AddLeftMargin;
        [SerializeField] private HorizontalOperationTypes operationRight = HorizontalOperationTypes.SubtractRightMargin;

        private Canvas canvasRoot;
        private RectTransform rectCanvasRoot;
        private Vector2 initialOffsetMax;
        private Vector2 initialOffsetMin;

        public event Action<SafeAreaApplier> OnPreBuild;
        public event Action<SafeAreaApplier> OnPostBuild;

        public bool IsInitialized { get; private set; }
        public bool IsBuilded { get; private set; }
        public bool IsUpdated => SafeArea == Screen.safeArea;
        public Rect SafeArea { get; private set; }

        private void OnValidate() {
            SafeArea = default;
            Build();
        }

        private void OnDestroy() {
            if (IsInitialized) {
                Canvas.willRenderCanvases -= Canvas_willRenderCanvases;
            }
        }

        private void Awake() {
            Initialize();
        }

        public void Initialize() {
            if (IsInitialized) {
                return;
            }

            canvasRoot = RectT.GetCanvasRoot();
            if (canvasRoot == null) {
                return;
            }

            rectCanvasRoot = canvasRoot.GetComponent<RectTransform>();
            if (rectCanvasRoot == null) {
                return;
            }

            initialOffsetMax = RectT.offsetMax;
            initialOffsetMin = RectT.offsetMin;

            Canvas.willRenderCanvases += Canvas_willRenderCanvases;
            IsInitialized = true;
        }

        public void Build() {
            if (!IsInitialized) {
                return;
            }

            if (operationTop == VerticalOperationTypes.None && operationBottom == VerticalOperationTypes.None && operationLeft == HorizontalOperationTypes.None && operationRight == HorizontalOperationTypes.None) {
                return;
            }

            if (SafeArea == Screen.safeArea) {
                return;
            }

            RenderMode renderMode = canvasRoot.renderMode;
            if (renderMode == RenderMode.WorldSpace) {
                return;
            }

            OnPreBuild?.Invoke(this);

            SafeArea = Screen.safeArea;
            float topMargin = GetTopMargin(SafeArea, rectCanvasRoot);
            float bottomMargin = GetBottomMargin(SafeArea, rectCanvasRoot);
            float leftMargin = GetLeftMargin(SafeArea, rectCanvasRoot);
            float rightMargin = GetRightMargin(SafeArea, rectCanvasRoot);

            Vector2 offsetMax;
            Vector2 offsetMin;

            offsetMax.x = GetOffsetValue(operationRight, leftMargin, rightMargin);
            offsetMax.y = GetOffsetValue(operationTop, topMargin, bottomMargin);

            offsetMin.x = GetOffsetValue(operationLeft, leftMargin, rightMargin);
            offsetMin.y = GetOffsetValue(operationBottom, topMargin, bottomMargin);

            RectT.offsetMax = initialOffsetMax + offsetMax;
            RectT.offsetMin = initialOffsetMin + offsetMin;

            IsBuilded = true;
            OnPostBuild?.Invoke(this);
        }

        public static float GetOffsetValue(VerticalOperationTypes operationType, float topMargin, float bottomMargin) {
            float offset = 0f;

            switch (operationType) {
                case VerticalOperationTypes.AddTopMargin:
                    offset += topMargin;
                    break;
                case VerticalOperationTypes.AddBottomMargin:
                    offset += bottomMargin;
                    break;
                case VerticalOperationTypes.SubtractTopMargin:
                    offset -= topMargin;
                    break;
                case VerticalOperationTypes.SubtractBottomMargin:
                    offset -= bottomMargin;
                    break;
            }

            return offset;
        }

        public static float GetOffsetValue(HorizontalOperationTypes operationType, float leftMargin, float rightMargin) {
            float offset = 0f;

            switch (operationType) {
                case HorizontalOperationTypes.AddLeftMargin:
                    offset = leftMargin;
                    break;
                case HorizontalOperationTypes.AddRightMargin:
                    offset = rightMargin;
                    break;
                case HorizontalOperationTypes.SubtractLeftMargin:
                    offset = -leftMargin;
                    break;
                case HorizontalOperationTypes.SubtractRightMargin:
                    offset = -rightMargin;
                    break;
            }

            return offset;
        }

        private void Canvas_willRenderCanvases() {
            Build();
        }

        public static bool GetSafeAreaMargins(RectTransform rectT, out float topMargin, out float bottomMargin) {
            topMargin = 0;
            bottomMargin = 0;

            Canvas canvasRoot = rectT.GetCanvasRoot();
            if (canvasRoot == null) {
                return false;
            }

            RectTransform rectCanvasRoot = canvasRoot.GetComponent<RectTransform>();
            if (rectCanvasRoot == null) {
                return false;
            }

            Rect safeArea = Screen.safeArea;
            topMargin = GetTopMargin(safeArea, rectCanvasRoot);
            bottomMargin = GetBottomMargin(safeArea, rectCanvasRoot);

            return true;
        }

        public static float GetTopMargin(Rect safeArea, RectTransform rectCanvasRoot) => (Screen.height - safeArea.position.y - safeArea.height) / Screen.height * rectCanvasRoot.rect.height;
        public static float GetBottomMargin(Rect safeArea, RectTransform rectCanvasRoot) => safeArea.position.y / Screen.height * rectCanvasRoot.rect.height;
        public static float GetLeftMargin(Rect safeArea, RectTransform rectCanvasRoot) => safeArea.position.x / Screen.width * rectCanvasRoot.rect.width;
        public static float GetRightMargin(Rect safeArea, RectTransform rectCanvasRoot) => (Screen.width - safeArea.position.x - safeArea.width) / Screen.width * rectCanvasRoot.rect.width;

        public enum VerticalOperationTypes {
            None,
            AddTopMargin,
            AddBottomMargin,
            SubtractTopMargin,
            SubtractBottomMargin,
        }

        public enum HorizontalOperationTypes {
            None,
            AddLeftMargin,
            AddRightMargin,
            SubtractLeftMargin,
            SubtractRightMargin,
        }
    }
}