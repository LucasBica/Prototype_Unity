using System;

using LB.Core.Runtime;
using LB.MessageSystem.Runtime;
using LB.Mvc.Runtime;
using LB.UIKit.Runtime.Components;

using UnityEngine;
using UnityEngine.EventSystems;

namespace LB.Prototype.Runtime.Mvc {

    public class UISettingsView : UIViewUpdater<ISettingsController, SettingsModel> {

        [Header("References")]
        [SerializeField] private UIToggle toggleSounds = default;
        [SerializeField] private UIToggle toggleMusic = default;
        [SerializeField] private UIButton buttonLogout = default;
        [SerializeField] private UIButton buttonCancel = default;

        protected override void Awake() {
            toggleSounds.OnIsOnValueChanged += ToggleSounds_OnIsOnValueChanged;
            toggleMusic.OnIsOnValueChanged += ToggleMusic_OnIsOnValueChanged;
            buttonLogout.OnClick += ButtonLogout_OnClick;
            buttonCancel.OnClick += ButtonCancel_OnClick;
        }

        public override void AttachIt(Action<IMessage> action) {
            Messenger.Attach(PrototypeCallbacks.UpdateSettings, action);
        }

        public override void DetachIt(Action<IMessage> action) {
            Messenger.Detach(PrototypeCallbacks.UpdateSettings, action);
        }

        protected override ISettingsController GetController() => DIContainer.Get<ISettingsController>();

        public override void UpdateView(SettingsModel model) {
            base.UpdateView(model);

            toggleSounds.IsOn = model.cacheModel.soundsIsOn;
            toggleMusic.IsOn = model.cacheModel.musicIsOn;
        }

        private void ToggleSounds_OnIsOnValueChanged(UIToggle toggle, bool isOn) {
            model.cacheModel.soundsIsOn = isOn;
            Controller.OnSoundsIsOnValueChanged(model);
        }

        private void ToggleMusic_OnIsOnValueChanged(UIToggle toggle, bool isOn) {
            model.cacheModel.musicIsOn = isOn;
            Controller.OnMusicIsOnValueChanged(model);
        }

        private void ButtonLogout_OnClick(UIButton button, PointerEventData eventData) {
            Controller.OnClickSignOut(model);
        }

        private void ButtonCancel_OnClick(UIButton button, PointerEventData eventData) {
            Controller.OnClickCancel(model);
        }
    }
}