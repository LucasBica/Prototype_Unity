using LB.UIKit.Runtime.Serializables;

using UnityEngine;
using UnityEngine.UI;

namespace LB.UIKit.Runtime.Components {

    public class UIViewControllerGraphic : UIViewControllerObserver<UIViewControllerGraphic> {

        [Header("References")]
        [SerializeField] protected SerializableArray<Graphic>[] graphicsArray = default;

        public SerializableArray<Graphic>[] GraphicsArray => graphicsArray;

        protected override void Awake() {
            for (int i = 0; i < graphicsArray.Length; i++) {
                if (graphicsArray[i][0].material == null) {
                    continue;
                }

                Material globalMaterial = graphicsArray[i][0].materialForRendering;
                Material instanceMaterial = new(globalMaterial);
                instanceMaterial.Lerp(instanceMaterial, globalMaterial, 1f);

                for (int j = 0; j < graphicsArray[i].Length; j++) {
                    graphicsArray[i][j].material = instanceMaterial;
                }
            }

            base.Awake();
        }

        protected override UIViewControllerGraphic GetThisInstance() => this;
    }
}