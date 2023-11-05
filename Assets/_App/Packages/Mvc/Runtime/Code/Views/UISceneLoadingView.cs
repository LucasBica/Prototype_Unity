using System;

using LB.Core.Runtime;
using LB.MessageSystem.Runtime;

using UnityEngine;

namespace LB.Mvc.Runtime {

    public class UISceneLoadingView : UIViewUpdater<ISceneLoadingController, SceneLoadingModel> {

        [Header("References")]
        [SerializeField] private Canvas canvas = default;
        [SerializeField] private GameObject root = default;

        protected override void OnDestroy() {
            base.OnDestroy();
            Messenger.Detach(MvcCallbacks.MainCameraChanged, MainCameraChanged);
        }

        protected override void Awake() {
            base.Awake();
            Messenger.Attach(MvcCallbacks.MainCameraChanged, MainCameraChanged);
        }

        public override void AttachIt(Action<IMessage> action) {
            Messenger.Attach(MvcCallbacks.UpdateLoadingView, action);
        }

        public override void DetachIt(Action<IMessage> action) {
            Messenger.Detach(MvcCallbacks.UpdateLoadingView, action);
        }

        protected override ISceneLoadingController GetController() => DIContainer.Get<ISceneLoadingController>();

        public override void UpdateView(SceneLoadingModel model) {
            if (this.model.shouldAppear) {
                this.model.shouldAppear = false;
                Appear(model.animated);

            } else if (this.model.shouldDisappear) {
                this.model.shouldDisappear = false;
                Disappear(model.animated);
            }

            root.SetActive(model.isVisible);
        }

        public override void Appear(bool animated) {
            ViewController.Appear(animated);
        }

        protected override void OnDidAppear() {
            base.OnDidAppear();
            Controller.OnDidAppear(model);
        }

        private void MainCameraChanged(IMessage<MvcCallbacks> message) {
            if (!message.GetContent(out Camera mainCamera)) {
                return;
            }

            canvas.worldCamera = mainCamera;
        }
    }
}