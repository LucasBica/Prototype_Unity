using UnityEngine;
using UnityEngine.UI;

namespace LB.UIKit.Runtime.Serializables {

    [CreateAssetMenu(fileName = nameof(UIVerticalColorVertex), menuName = UIKitConsts.PATH_ASSET_COLORS + nameof(UIVerticalColorVertex))]
    public class UIVerticalColorVertex : UIColorVertexBase {

        [SerializeField] protected Color colorTop = Color.white;
        [SerializeField] protected Color colorBottom = Color.white;

        public Color ColorTop => colorTop;
        public Color ColorBottom => colorBottom;

        public override void ModifyMesh(Rect rect, VertexHelper vertexHelper) {
            if (rect.height == 0f || vertexHelper == null) {
                return;
            }

            UIVertex vertex = default;
            for (int i = 0; i < vertexHelper.currentVertCount; i++) {
                vertexHelper.PopulateUIVertex(ref vertex, i);
                vertex.color *= Color.LerpUnclamped(colorBottom, colorTop, (vertex.position.y / rect.height) + 0.5f); // The division will give a number between -0.5f and 0.5f. We add 0.5f to convert it to 0f and 1f
                vertexHelper.SetUIVertex(vertex, i);
            }
        }
    }
}