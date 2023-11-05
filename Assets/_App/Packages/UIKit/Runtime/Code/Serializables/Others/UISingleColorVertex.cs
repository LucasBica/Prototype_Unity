using UnityEngine;
using UnityEngine.UI;

namespace LB.UIKit.Runtime.Serializables {

    [CreateAssetMenu(fileName = nameof(UISingleColorVertex), menuName = UIKitConsts.PATH_ASSET_COLORS + nameof(UISingleColorVertex))]
    public class UISingleColorVertex : UIColorVertexBase {

        [SerializeField] protected Color color = Color.white;

        public Color Color => color;

        public override void ModifyMesh(Rect rect, VertexHelper vertexHelper) {
            if (vertexHelper == null) {
                return;
            }

            UIVertex vertex = default;
            for (int i = 0; i < vertexHelper.currentVertCount; i++) {
                vertexHelper.PopulateUIVertex(ref vertex, i);
                vertex.color *= color;
                vertexHelper.SetUIVertex(vertex, i);
            }
        }
    }
}