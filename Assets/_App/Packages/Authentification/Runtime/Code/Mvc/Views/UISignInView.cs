using System;

using LB.Core.Runtime;
using LB.MessageSystem.Runtime;
using LB.UIKit.Runtime.Components;
using LB.UIKit.Runtime.Serializables;

using UnityEngine;
using UnityEngine.EventSystems;

namespace LB.Authentication.Runtime.Mvc {

    public class UISignInView : UIAuthenticationView<ISignInController, AuthenticationModel> {

        [Header("Assets")]
        [SerializeField] private UITransitionCombinatorViewController transitionDefault = default;
        [SerializeField] private UITransitionCombinatorViewController transitionRecoveryPassword = default;
        [SerializeField] private UITransitionCombinatorViewController transitionAuthentication = default;

        [Header("References")]
        [SerializeField] private UIButton buttonForgotPassword = default;

        protected override void Awake() {
            base.Awake();

            buttonForgotPassword.OnClick += ButtonForgotPassword_OnClick;
        }

        protected override ISignInController GetController() => DIContainer.Get<ISignInController>();

        public override void AttachIt(Action<IMessage> action) {
            Messenger.Attach(AuthenticationCallbacks.UpdateSignIn, action);
        }

        public override void DetachIt(Action<IMessage> action) {
            Messenger.Detach(AuthenticationCallbacks.UpdateSignIn, action);
        }

        private void ButtonForgotPassword_OnClick(UIButton button, PointerEventData eventData) {
            ViewController.SetTransitionAppear(transitionRecoveryPassword);
            ViewController.SetTransitionDisappear(transitionRecoveryPassword);

            Controller.OnClickForgotPassword(model);
        }

        protected override void ButtonAuthentication_OnClick(UIButton button, PointerEventData eventData) {
            ViewController.SetTransitionAppear(transitionAuthentication);
            ViewController.SetTransitionDisappear(transitionAuthentication);

            Controller.OnClickSignIn(model);
        }

        protected override void ButtonAlternativeView_OnClick(UIButton button, PointerEventData eventData) {
            ViewController.SetTransitionAppear(transitionDefault);
            ViewController.SetTransitionDisappear(transitionDefault);

            Controller.OnClickAppearSignUp(model);
        }
    }
}