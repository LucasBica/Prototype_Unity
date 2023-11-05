using LB.UIKit.Runtime.Components;

using UnityEngine;
using UnityEngine.UI;

namespace LB.UIKit.Runtime.Serializables {

    [CreateAssetMenu(fileName = nameof(UIFourCornersColorVertex), menuName = UIKitConsts.PATH_ASSET_COLORS + nameof(UIFourCornersColorVertex))]
    public class UIFourCornersColorVertex : UIColorVertexBase {

        [SerializeField] protected Color colorTopLeft = Color.white;
        [SerializeField] protected Color colorTopRight = Color.white;
        [SerializeField] protected Color colorBottomLeft = Color.white;
        [SerializeField] protected Color colorBottomRight = Color.white;

        public Color ColorTopLeft => colorTopLeft;
        public Color ColorTopRight => colorTopRight;
        public Color ColorBottomLeft => colorBottomLeft;
        public Color ColorBottomRight => colorBottomRight;

        public override void ModifyMesh(Rect rect, VertexHelper vertexHelper) {
            if (rect.width == 0f || rect.height == 0f || vertexHelper == null) {
                return;
            }

            UIVertex vertex = default;
            for (int i = 0; i < vertexHelper.currentVertCount; i++) {
                vertexHelper.PopulateUIVertex(ref vertex, i);
                Vector2 positionNormalized = new Vector2((vertex.position.x / rect.width) + 0.5f, (vertex.position.y / rect.height) + 0.5f); // The division will give a number between -0.5f and 0.5f. We add 0.5f to convert it to 0f and 1f

                Color colorTop = Color.LerpUnclamped(colorTopLeft, colorTopRight, positionNormalized.x);
                Color colorBotton = Color.LerpUnclamped(colorBottomLeft, colorBottomRight, positionNormalized.x);
                Color color = Color.LerpUnclamped(colorBotton, colorTop, positionNormalized.y);

                vertex.color *= color;
                vertexHelper.SetUIVertex(vertex, i);
            }
        }
    }
}