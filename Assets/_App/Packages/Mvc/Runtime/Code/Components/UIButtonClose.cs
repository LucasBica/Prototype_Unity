using LB.Core.Runtime;
using LB.UIKit.Runtime.Components;

using UnityEngine;
using UnityEngine.EventSystems;

namespace LB.Mvc.Runtime.Components {

    public class UIButtonClose : UIView {

        [Header("References")]
        [SerializeField] private UIButton button = default;

        private IUINavigator navigator;

        private void Awake() {
            navigator = DIContainer.Get<IUINavigator>();
            button.OnClick += Button_OnClick;
        }

        private void Button_OnClick(UIButton button, PointerEventData eventData) {
            navigator.PopViewAndModel();
        }
    }
}