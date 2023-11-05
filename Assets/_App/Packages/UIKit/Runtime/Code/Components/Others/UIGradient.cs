using LB.UIKit.Runtime.Serializables;

using UnityEngine;
using UnityEngine.UI;

namespace LB.UIKit.Runtime.Components {

    public class UIGradient : BaseMeshEffect {

        [Header("Settings")]
        [SerializeField] protected UIColorVertexBase colorVertex = default;

        private RectTransform rectT;
        public RectTransform RectT {
            get {
                if (rectT == null) {
                    rectT = GetComponent<RectTransform>();
                }
                return rectT;
            }
        }

#if UNITY_EDITOR
        protected override void OnValidate() {
            base.OnValidate();

            Graphic graphic = GetComponent<Graphic>();
            if (graphic == null) {
                Debug.LogError($"[{nameof(UIGradient)}] This component require a {nameof(Graphic)}. For example an {nameof(Image)}");
            }
        }
#endif

        public override void ModifyMesh(VertexHelper vertexHelper) {
            if (!isActiveAndEnabled || colorVertex == null) {
                return;
            }

            colorVertex.ModifyMesh(RectT.rect, vertexHelper);
        }
    }
}