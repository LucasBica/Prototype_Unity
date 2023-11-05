using UnityEngine;
using UnityEngine.UI;

namespace LB.UIKit.Runtime.Serializables {

    [CreateAssetMenu(fileName = nameof(UIHorizontalColorVertex), menuName = UIKitConsts.PATH_ASSET_COLORS + nameof(UIHorizontalColorVertex))]
    public class UIHorizontalColorVertex : UIColorVertexBase {

        [SerializeField] protected Color colorLeft = Color.white;
        [SerializeField] protected Color colorRight = Color.white;

        public Color ColorLeft => colorLeft;
        public Color ColorRight => colorRight;

        public override void ModifyMesh(Rect rect, VertexHelper vertexHelper) {
            if (rect.width == 0f || vertexHelper == null) {
                return;
            }

            UIVertex vertex = default;
            for (int i = 0; i < vertexHelper.currentVertCount; i++) {
                vertexHelper.PopulateUIVertex(ref vertex, i);
                vertex.color *= Color.LerpUnclamped(colorLeft, colorRight, (vertex.position.x / rect.width) + 0.5f); // The division will give a number between -0.5f and 0.5f. We add 0.5f to convert it to 0f and 1f
                vertexHelper.SetUIVertex(vertex, i);
            }
        }
    }
}