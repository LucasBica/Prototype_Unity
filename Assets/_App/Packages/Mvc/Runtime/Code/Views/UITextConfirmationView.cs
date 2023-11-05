using System;

using LB.Core.Runtime;
using LB.MessageSystem.Runtime;
using LB.UIKit.Runtime.Components;

using TMPro;

using UnityEngine;
using UnityEngine.EventSystems;

namespace LB.Mvc.Runtime {

    public class UITextConfirmationView : UIViewUpdater<ITextConfirmationController, TextConfirmationModel> {

        [Header("References")]
        [SerializeField] private ContentSizeController contentSizeController;
        [SerializeField] private SwitchGameObjects switchGameObjects;
        [SerializeField] private TMP_Text textTitle = default;
        [SerializeField] private TMP_InputField inputText = default;
        [Space]
        [SerializeField] private UIButton buttonFirst = default;
        [SerializeField] private TMP_Text textButtonFirst = default;
        [SerializeField] private UIButton buttonSecond = default;
        [SerializeField] private TMP_Text textButtonSecond = default;

        protected override void Awake() {
            base.Awake();

            inputText.onValueChanged.AddListener(InputText_onValueChanged);
            buttonFirst.OnClick += ButtonFirst_OnClick;
            buttonSecond.OnClick += ButtonSecond_OnClick;
        }

        protected override ITextConfirmationController GetController() => DIContainer.Get<ITextConfirmationController>();

        public override void AttachIt(Action<IMessage> action) {
            Messenger.Attach(MvcCallbacks.UpdateTextConfirmationView, action);
        }

        public override void DetachIt(Action<IMessage> action) {
            Messenger.Detach(MvcCallbacks.UpdateTextConfirmationView, action);
        }

        public override void UpdateView(TextConfirmationModel model) {
            base.UpdateView(model);

            switchGameObjects.SetState(model.isLoading);
            textTitle.text = model.title;

            if (inputText.placeholder != null && inputText.placeholder is TMP_Text textPlaceholder) {
                textPlaceholder.text = model.placeholder;
            }

            inputText.text = model.inputText;

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

        private void InputText_onValueChanged(string text) {
            model.inputText = text;
            Controller.OnInputTextChangedValue(model);
        }

        private void ButtonFirst_OnClick(UIButton button, PointerEventData eventData) {
            Controller.OnClickButtonFirst(model);
        }

        private void ButtonSecond_OnClick(UIButton button, PointerEventData eventData) {
            Controller.OnClickButtonSecond(model);
        }
    }
}