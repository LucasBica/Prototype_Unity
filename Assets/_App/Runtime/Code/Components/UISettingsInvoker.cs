using LB.Core.Runtime;
using LB.Prototype.Runtime.Mvc;
using LB.UIKit.Runtime.Components;
using LB.UIKit.Runtime.Serializables;

using UnityEngine;
using UnityEngine.EventSystems;

namespace LB.Prototype.Runtime {

    public class UISettingsInvoker : UIView {

        [Header("Assets")]
        [SerializeField] private UITransitionCombinatorViewController transition = default;

        [Header("References")]
        [SerializeField] private UIViewController viewController = default;
        [SerializeField] private UIButton button = default;

        private ISettingsController settingsController;

        private void Awake() {
            settingsController = DIContainer.Get<ISettingsController>();
            button.OnClick += Button_OnClick;
        }

        private void Button_OnClick(UIButton button, PointerEventData eventData) {
            viewController.SetTransitionAppear(transition);
            viewController.SetTransitionDisappear(transition);

            settingsController.OnClickAppear();
        }
    }
}