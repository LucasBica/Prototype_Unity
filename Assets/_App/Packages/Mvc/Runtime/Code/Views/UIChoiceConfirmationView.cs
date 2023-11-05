using System;

using LB.Core.Runtime;
using LB.MessageSystem.Runtime;
using LB.UIKit.Runtime.Components;

using TMPro;

using UnityEngine;
using UnityEngine.EventSystems;

namespace LB.Mvc.Runtime {

    public class UIChoiceConfirmationView : UIViewUpdater<IChoiceConfirmationController, ChoiceConfirmationModel> {

        [Header("References")]
        [SerializeField] private ContentSizeController contentSizeController;
        [SerializeField] private SwitchGameObjects switchGameObjects;
        [SerializeField] private TMP_Text textTitle = default;
        [SerializeField] private TMP_Text textDescription = default;
        [Space]
        [SerializeField] private UIButton buttonFirst = default;
        [SerializeField] private TMP_Text textButtonFirst = default;
        [SerializeField] private UIButton buttonSecond = default;
        [SerializeField] private TMP_Text textButtonSecond = default;

        protected override void Awake() {
            base.Awake();

            buttonFirst.OnClick += ButtonFirst_OnClick;
            buttonSecond.OnClick += ButtonSecond_OnClick;
        }

        public override void AttachIt(Action<IMessage> action) {
            Messenger.Attach(MvcCallbacks.UpdateChoiceConfirmationView, action);
        }

        public override void DetachIt(Action<IMessage> action) {
            Messenger.Detach(MvcCallbacks.UpdateChoiceConfirmationView, action);
        }

        protected override IChoiceConfirmationController GetController() => DIContainer.Get<IChoiceConfirmationController>();

        public override void UpdateView(ChoiceConfirmationModel model) {
            base.UpdateView(model);

            switchGameObjects.SetState(model.isLoading);

            textTitle.text = model.title;
            textDescription.text = model.description;

            textButtonFirst.text = model.textFirstButton;
            textButtonSecond.text = model.textSecondButton;

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

        private void ButtonFirst_OnClick(UIButton button, PointerEventData eventData) {
            Controller.OnClickButtonFirst(model);
        }

        private void ButtonSecond_OnClick(UIButton button, PointerEventData eventData) {
            Controller.OnClickButtonSecond(model);
        }
    }
}