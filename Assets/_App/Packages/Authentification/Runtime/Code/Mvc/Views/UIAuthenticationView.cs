using LB.Mvc.Runtime;
using LB.UIKit.Runtime.Components;

using TMPro;

using UnityEngine;
using UnityEngine.EventSystems;

namespace LB.Authentication.Runtime.Mvc {

    public abstract class UIAuthenticationView<TController, TModel> : UIViewUpdater<TController, TModel> where TController : IAuthenticationController, IController<TModel> where TModel : AuthenticationModel, new() {

        [Header("References")]
        [SerializeField] private SwitchGameObjects switchGameObjects = default;
        [SerializeField] private TMP_InputField inputTextEmail = default;
        [SerializeField] private TMP_InputField inputTextPassword = default;

        [SerializeField] private UIButton buttonAuthentication = default;
        [Space]
        [SerializeField] private UIButton buttonGoogle = default;
        [SerializeField] private UIButton buttonApple = default;
        [SerializeField] private UIButton buttonMicrosoft = default;
        [Space]
        [SerializeField] private UIButton buttonAlternativeView;

        protected override void Awake() {
            base.Awake();

            inputTextEmail.onValueChanged.AddListener(InputTextEmail_onValueChanged);
            inputTextPassword.onValueChanged.AddListener(InputTextPassword_onValueChanged);
            buttonAuthentication.OnClick += ButtonAuthentication_OnClick;

            buttonGoogle.OnClick += ButtonGoogle_OnClick;
            buttonApple.OnClick += ButtonApple_OnClick;
            buttonMicrosoft.OnClick += ButtonMicrosoft_OnClick;

            buttonAlternativeView.OnClick += ButtonAlternativeView_OnClick;
        }

        public override void UpdateView(TModel model) {
            base.UpdateView(model);

            switchGameObjects.SetState(model.isLoading);
            inputTextEmail.text = this.model.email;
            inputTextPassword.text = this.model.password;
        }

        protected override void OnWillAppear() {
            base.OnWillAppear();
            Controller.OnWillAppear(model);
        }

        private void InputTextEmail_onValueChanged(string text) {
            model.email = text;
        }

        private void InputTextPassword_onValueChanged(string text) {
            model.password = text;
        }

        protected abstract void ButtonAuthentication_OnClick(UIButton button, PointerEventData eventData);

        private void ButtonGoogle_OnClick(UIButton button, PointerEventData eventData) {
            Controller.OnClickGoogle(model);
        }

        private void ButtonApple_OnClick(UIButton button, PointerEventData eventData) {
            Controller.OnClickApple(model);
        }

        private void ButtonMicrosoft_OnClick(UIButton button, PointerEventData eventData) {
            Controller.OnClickMicrosoft(model);
        }

        protected abstract void ButtonAlternativeView_OnClick(UIButton button, PointerEventData eventData);
    }
}