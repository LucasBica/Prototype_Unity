using System;

using LB.Core.Runtime;
using LB.MessageSystem.Runtime;
using LB.Mvc.Runtime;
using LB.UIKit.Runtime.Components;
using LB.UIKit.Runtime.Serializables;

using TMPro;

using UnityEngine;
using UnityEngine.EventSystems;

namespace LB.Authentication.Runtime.Mvc {

    public class UISignUpView : UIAuthenticationView<ISignUpController, AuthenticationModel> {

        [Header("Assets")]
        [SerializeField] private UITransitionCombinatorViewController transitionDefault = default;
        [SerializeField] private UITransitionCombinatorViewController transitionAuthentication = default;

        [Header("References")]
        [SerializeField] private TMP_InputField inputTextUsername = default;

        protected override void Awake() {
            base.Awake();

            inputTextUsername.onValueChanged.AddListener(InputTextUsername_onValueChanged);
        }

        protected override ISignUpController GetController() => DIContainer.Get<ISignUpController>();

        public override void AttachIt(Action<IMessage> action) {
            Messenger.Attach(AuthenticationCallbacks.UpdateSignUp, action);
        }

        public override void DetachIt(Action<IMessage> action) {
            Messenger.Detach(AuthenticationCallbacks.UpdateSignUp, action);
        }

        private void InputTextUsername_onValueChanged(string text) {
            model.username = text;
        }

        protected override void ButtonAuthentication_OnClick(UIButton button, PointerEventData eventData) {
            ViewController.SetTransitionAppear(transitionAuthentication);
            ViewController.SetTransitionDisappear(transitionAuthentication);

            Controller.OnClickSignUp(model);
        }

        protected override void ButtonAlternativeView_OnClick(UIButton button, PointerEventData eventData) {
            ViewController.SetTransitionAppear(transitionDefault);
            ViewController.SetTransitionDisappear(transitionDefault);

            Controller.OnClickAppearSignIn(model);
        }
    }
}