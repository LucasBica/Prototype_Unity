using LB.UIKit.Runtime.Components;

using UnityEngine;
using UnityEngine.UI;

namespace LB.UIKit.Runtime.Serializables {

    [CreateAssetMenu(fileName = nameof(UITransitionViewControllerGraphic_Material), menuName = UIKitConsts.PATH_ASSET_TRANSITION + nameof(UIViewControllerGraphic) + "/" + nameof(UITransitionViewControllerGraphic_Material))]
    public class UITransitionViewControllerGraphic_Material : UITransitionViewControllerLerp<UIViewControllerGraphic, Material[]> {

        protected override Material[] Lerp(UIViewControllerGraphic view, Material[] from, Material[] to, float time) {
            int length = Mathf.Min(from.Length, to.Length);
            Material[] materials = new Material[length];

            for (int i = 0; i < length; i++) {
                if (view.GraphicsArray[i][0].material == null) {
                    continue;
                }

                materials[i] = view.GraphicsArray[i][0].material;
                materials[i].Lerp(from[i], to[i], time);
            }

            return materials;
        }

        protected override void SetCurrentValue(UIViewControllerGraphic view, Material[] value) {
            for (int i = 0; i < Mathf.Min(value.Length, view.GraphicsArray.Length); i++) {
                Graphic[] graphics = view.GraphicsArray[i].array;
                for (int j = 0; j < graphics.Length; j++) {
                    if (graphics[j] == null) { // This could happen when in editor the developer is editing the graphic references.
                        continue;
                    }

                    graphics[j].SetMaterialDirty();
                }
            }
        }
    }
}