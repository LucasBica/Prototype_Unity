using LB.UIKit.Runtime.Components;

using UnityEngine;
using UnityEngine.UI;

namespace LB.UIKit.Runtime.Serializables {

    [CreateAssetMenu(fileName = nameof(UITransitionButtonGraphic_Color), menuName = UIKitConsts.PATH_ASSET_TRANSITION + nameof(UIButtonGraphic) + "/" + nameof(UITransitionButtonGraphic_Color))]
    public class UITransitionButtonGraphic_Color : UITransitionButtonLerp<UIButtonGraphic, Color[]> {

        protected override bool IsInteractive(UIButtonGraphic buttonGraphic) {
            return buttonGraphic.Subject.IsInteractive;
        }

        protected override ButtonParameter<Color[]> GetParameter(UIButtonGraphic buttonGraphic) {
            return buttonStatesParams.GetByState(buttonGraphic.Subject.State);
        }

        protected override Color[] GetFromValue(UIButtonGraphic buttonGraphic) {
            object data = buttonGraphic.Subject.GetTransitionData(this);
            return data == null ? GetCurrentValue(buttonGraphic) : (Color[])data;
        }

        protected override void SetFromValue(UIButtonGraphic buttonGraphic) {
            object data = GetCurrentValue(buttonGraphic);
            buttonGraphic.Subject.SetTransitionData(this, data);
        }

        protected virtual Color[] GetCurrentValue(UIButtonGraphic buttonGraphic) {
            Color[] currentColors = new Color[buttonGraphic.GraphicsArray.Length];

            for (int i = 0; i < buttonGraphic.GraphicsArray.Length; i++) {
                currentColors[i] = buttonGraphic.GraphicsArray[i][0].canvasRenderer.GetColor();
            }

            return currentColors;
        }

        protected override void SetCurrentValue(UIButtonGraphic buttonGraphic, Color[] colors) {
            for (int i = 0; i < Mathf.Min(colors.Length, buttonGraphic.GraphicsArray.Length); i++) {
                Graphic[] graphics = buttonGraphic.GraphicsArray[i].array;
                Color color = colors[i];
                for (int j = 0; j < graphics.Length; j++) {
                    if (graphics[j] == null) { // This could happen when in editor the developer is editing the graphic references.
                        continue;
                    }
                    graphics[j].canvasRenderer.SetColor(color);
                }
            }
        }

        protected override Color[] Lerp(UIButtonGraphic buttonGraphic, Color[] from, Color[] to, float time) {
            int length = Mathf.Min(from.Length, to.Length);
            Color[] colors = new Color[length];

            for (int i = 0; i < length; i++) {
                colors[i] = Color.LerpUnclamped(from[i], to[i], time);
            }

            return colors;
        }
    }
}