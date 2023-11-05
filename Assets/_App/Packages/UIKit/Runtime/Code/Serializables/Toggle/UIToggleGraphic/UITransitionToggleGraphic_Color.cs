using LB.UIKit.Runtime.Components;

using UnityEngine;
using UnityEngine.UI;

namespace LB.UIKit.Runtime.Serializables {

    [CreateAssetMenu(fileName = nameof(UITransitionToggleGraphic_Color), menuName = UIKitConsts.PATH_ASSET_TRANSITION + nameof(UIToggleGraphic) + "/" + nameof(UITransitionToggleGraphic_Color))]
    public class UITransitionToggleGraphic_Color : UITransitionToggleLerp<UIToggleGraphic, Color[]> {

        protected override bool IsOn(UIToggleGraphic view) {
            return view.Subject.IsOn;
        }

        protected override ToggleParameter<Color[]> GetParameter(UIToggleGraphic view) {
            return toggleStatesParams.GetByState(view.Subject.IsOn ? UIToggle.States.IsOn : UIToggle.States.IsOff);
        }

        protected override Color[] GetFromValue(UIToggleGraphic view) {
            object data = view.Subject.GetTransitionData(this);
            return data == null ? GetCurrentValue(view) : (Color[])data;
        }

        protected override void SetFromValue(UIToggleGraphic view) {
            object data = GetCurrentValue(view);
            view.Subject.SetTransitionData(this, data);
        }

        protected virtual Color[] GetCurrentValue(UIToggleGraphic view) {
            Color[] currentColors = new Color[view.GraphicsArray.Length];

            for (int i = 0; i < view.GraphicsArray.Length; i++) {
                currentColors[i] = view.GraphicsArray[i][0].color;
            }

            return currentColors;
        }

        protected override void SetCurrentValue(UIToggleGraphic view, Color[] value) {
            for (int i = 0; i < Mathf.Min(value.Length, view.GraphicsArray.Length); i++) {
                Graphic[] graphics = view.GraphicsArray[i].array;
                Color color = value[i];
                for (int j = 0; j < graphics.Length; j++) {
                    if (graphics[j] == null) { // This could happen when in editor the developer is editing the graphic references.
                        continue;
                    }
                    graphics[j].color = color;
                }
            }
        }

        protected override Color[] Lerp(UIToggleGraphic view, Color[] from, Color[] to, float time) {
            int length = Mathf.Min(from.Length, to.Length);
            Color[] colors = new Color[length];

            for (int i = 0; i < length; i++) {
                colors[i] = Color.LerpUnclamped(from[i], to[i], time);
            }

            return colors;
        }
    }
}