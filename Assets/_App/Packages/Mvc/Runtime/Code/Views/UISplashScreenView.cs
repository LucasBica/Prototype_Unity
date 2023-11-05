using System;

using LB.Core.Runtime;
using LB.MessageSystem.Runtime;

using UnityEngine.Video;
using UnityEngine;

namespace LB.Mvc.Runtime {

    public class UISplashScreenView : UIViewUpdater<ISplashScreenController, SplashScreenModel> {

        [Header("Assets")]
        [SerializeField] protected VideoClip[] videos = default;

        [Header("References")]
        [SerializeField] protected VideoPlayer videoPlayer = default;

        protected override void Awake() {
            base.Awake();
            videoPlayer.loopPointReached += VideoPlayer_loopPointReached;
        }

        public override void AttachIt(Action<IMessage> action) {
            Messenger.Attach(MvcCallbacks.UpdateSplashScreen, action);
        }

        public override void DetachIt(Action<IMessage> action) {
            Messenger.Detach(MvcCallbacks.UpdateSplashScreen, action);
        }

        protected override ISplashScreenController GetController() => DIContainer.Get<ISplashScreenController>();

        public override void UpdateView(SplashScreenModel model) {
            if (this.model.shouldAppear) {
                this.model.shouldAppear = false;
                gameObject.SetActive(true);

            } else if (this.model.shouldDisappear) {
                this.model.shouldDisappear = false;
                gameObject.SetActive(false);
            }

            if (model.splashScreenIndex >= 0 && model.splashScreenIndex < videos.Length) {
                videoPlayer.clip = videos[model.splashScreenIndex];
                videoPlayer.Play();
            } else {
                Controller.OnCompletedAll(model);
            }
        }

        private void VideoPlayer_loopPointReached(VideoPlayer videoPlayer) {
            Controller.OnCompletedOne(model);
        }
    }
}