using LB.UIKit.Runtime.Components;

using UnityEngine;
using UnityEngine.UI;

namespace LB.UIKit.Runtime.Serializables {

    [CreateAssetMenu(fileName = nameof(UITransitionButtonGraphic_Material), menuName = UIKitConsts.PATH_ASSET_TRANSITION + nameof(UIButtonGraphic) + "/" + nameof(UITransitionButtonGraphic_Material))]
    public class UITransitionButtonGraphic_Material : UITransitionButtonLerp<UIButtonGraphic, Material[]> {

        public override void OnValidateView(UIButtonGraphic buttonGraphic) {
            Material[] materials = IsInteractive(buttonGraphic) ? buttonStatesParams.normal.value : buttonStatesParams.disabled.value;

            for (int i = 0; i < Mathf.Min(materials.Length, buttonGraphic.GraphicsArray.Length); i++) {
                Graphic[] graphics = buttonGraphic.GraphicsArray[i].array;
                for (int j = 0; j < graphics.Length; j++) {
                    if (graphics[j] == null) { // This could happen when in editor the developer is editing the graphic references.
                        continue;
                    }

                    graphics[j].material = materials[j];
                }
            }
        }

        protected override bool IsInteractive(UIButtonGraphic buttonGraphic) {
            return buttonGraphic.Subject.IsInteractive;
        }

        protected override ButtonParameter<Material[]> GetParameter(UIButtonGraphic buttonGraphic) {
            return buttonStatesParams.GetByState(buttonGraphic.Subject.State);
        }

        protected override Material[] GetFromValue(UIButtonGraphic buttonGraphic) {
            object data = buttonGraphic.GetTransitionData(this);

            if (data == null) {
                SetFromValue(buttonGraphic);
                data = buttonGraphic.GetTransitionData(this);
            }

            return data == null ? GetCurrentValue(buttonGraphic) : (Material[])data;
        }

        protected override void SetFromValue(UIButtonGraphic buttonGraphic) {
            Material[] currentMaterials = GetCurrentValue(buttonGraphic);
            object data = buttonGraphic.GetTransitionData(this);
            Material[] fromMaterials;

            if (data == null) {
                fromMaterials = new Material[currentMaterials.Length];
                for (int i = 0; i < currentMaterials.Length; i++) {
                    fromMaterials[i] = new Material(currentMaterials[i]);
                }
            } else {
                fromMaterials = (Material[])data;

                if (fromMaterials.Length < currentMaterials.Length) {
                    Material[] newMaterials = new Material[currentMaterials.Length];
                    for (int i = 0; i < currentMaterials.Length; i++) {
                        newMaterials[i] = i < fromMaterials.Length && fromMaterials[i] != null ? fromMaterials[i] : new Material(currentMaterials[i]);
                    }
                    fromMaterials = newMaterials;
                }

            }

            for (int i = 0; i < currentMaterials.Length; i++) {
                fromMaterials[i].Lerp(fromMaterials[i], currentMaterials[i], 1f);
            }

            data = fromMaterials;
            buttonGraphic.SetTransitionData(this, data);
        }

        protected virtual Material[] GetCurrentValue(UIButtonGraphic buttonGraphic) {
            Material[] currentMaterials = new Material[buttonGraphic.GraphicsArray.Length];

            for (int i = 0; i < buttonGraphic.GraphicsArray.Length; i++) {
                currentMaterials[i] = buttonGraphic.GraphicsArray[i][0].materialForRendering;
            }

            return currentMaterials;
        }

        protected override void SetCurrentValue(UIButtonGraphic buttonGraphic, Material[] materials) {
            for (int i = 0; i < Mathf.Min(materials.Length, buttonGraphic.GraphicsArray.Length); i++) {
                Graphic[] graphics = buttonGraphic.GraphicsArray[i].array;
                for (int j = 0; j < graphics.Length; j++) {
                    if (graphics[j] == null) { // This could happen when in editor the developer is editing the graphic references.
                        continue;
                    }

                    graphics[j].SetMaterialDirty();
                }
            }
        }

        protected override Material[] Lerp(UIButtonGraphic buttonGraphic, Material[] from, Material[] to, float time) {
            int length = Mathf.Min(from.Length, to.Length);
            Material[] materials = new Material[length];

            for (int i = 0; i < length; i++) {
                if (buttonGraphic.GraphicsArray[i][0].material == null) {
                    continue;
                }

                materials[i] = buttonGraphic.GraphicsArray[i][0].material;
                materials[i].Lerp(from[i], to[i], time);
            }

            return materials;
        }
    }
}
