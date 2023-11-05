using System;

using LB.Core.Runtime;
using LB.MessageSystem.Runtime;
using LB.UIKit.Runtime.Components;

using TMPro;

using UnityEngine;
using UnityEngine.EventSystems;

namespace LB.Mvc.Runtime {

    public class UIAlertView : UIViewUpdater<IAlertController, AlertModel> {

        [Header("References")]
        [SerializeField] private ContentSizeController contentSizeController = default;
        [SerializeField] private SwitchGameObjects switchGameObjects = default;
        [Space]
        [SerializeField] private TMP_Text textTitle = default;
        [SerializeField] private TMP_Text textDescription = default;
        [Space]
        [SerializeField] private UIButton button = default;
        [SerializeField] private TMP_Text textButton = default;

        protected override void Awake() {
            base.Awake();

            button.OnClick += Button_OnClick;
        }

        protected override IAlertController GetController() => DIContainer.Get<IAlertController>();

        public override void AttachIt(Action<IMessage> action) {
            Messenger.Attach(MvcCallbacks.UpdateAlertView, action);
        }

        public override void DetachIt(Action<IMessage> action) {
            Messenger.Detach(MvcCallbacks.UpdateAlertView, action);
        }

        public override void UpdateView(AlertModel model) {
            base.UpdateView(model);

            switchGameObjects.SetState(model.isLoading);

            textTitle.text = model.title;
            textDescription.text = model.description;
            textButton.text = model.textButton;

            contentSizeController.ForceReBuild();
        }

        protected override void OnWillAppear() {
            base.OnWillAppear();
            Controller.OnWillAppear(model);
        }

        protected override void OnWillDisappear() {
            base.OnWillDisappear();
            Controller.OnWillDisappear(model);
        }

        private void Button_OnClick(UIButton button, PointerEventData eventData) {
            Controller.OnClickButton(model);
        }
    }
}