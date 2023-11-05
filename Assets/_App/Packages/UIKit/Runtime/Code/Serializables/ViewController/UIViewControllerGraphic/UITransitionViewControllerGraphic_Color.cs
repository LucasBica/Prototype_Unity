using LB.UIKit.Runtime.Components;

using UnityEngine;
using UnityEngine.UI;

namespace LB.UIKit.Runtime.Serializables {

    [CreateAssetMenu(fileName = nameof(UITransitionViewControllerGraphic_Color), menuName = UIKitConsts.PATH_ASSET_TRANSITION + nameof(UIViewControllerGraphic) + "/" + nameof(UITransitionViewControllerGraphic_Color))]
    public class UITransitionViewControllerGraphic_Color : UITransitionViewControllerLerp<UIViewControllerGraphic, Color[]> {

        protected override Color[] Lerp(UIViewControllerGraphic view, Color[] from, Color[] to, float time) {
            int length = Mathf.Min(from.Length, to.Length);
            Color[] colors = new Color[length];

            for (int i = 0; i < length; i++) {
                colors[i] = Color.LerpUnclamped(from[i], to[i], time);
            }

            return colors;
        }

        protected override void SetCurrentValue(UIViewControllerGraphic view, Color[] value) {
            int length = Mathf.Min(view.GraphicsArray.Length, value.Length);

            for (int i = 0; i < length; i++) {
                SerializableArray<Graphic> graphicsArray = view.GraphicsArray[i];

                for (int j = 0; j < graphicsArray.Length; j++) {
                    Graphic graphic = graphicsArray[j];
                    graphic.color = value[i];
                }
            }
        }
    }
}