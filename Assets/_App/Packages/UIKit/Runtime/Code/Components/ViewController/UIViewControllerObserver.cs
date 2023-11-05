using LB.TweenSystem.Runtime;
using LB.UIKit.Runtime.Components;

using UnityEngine;

namespace LB.UIKit.Runtime.Serializables {

    [RequireComponent(typeof(UIViewController))]
    public abstract class UIViewControllerObserver<T> : UIObserver<UIViewController> where T : UIView {

        [Header("Assets")]
        [SerializeField] protected UITransitionCombinatorViewControllerBase<T> transitionReset = default;
        [SerializeField] protected UITransitionCombinatorViewControllerBase<T> transitionAppear = default;
        [SerializeField] protected UITransitionCombinatorViewControllerBase<T> transitionDisappear = default;

        public UITransitionCombinatorViewControllerBase<T> CurrentTransition => Subject.State == UIViewController.States.Appearing ? transitionAppear : Subject.State == UIViewController.States.Disappearing ? transitionDisappear : null;

        public UITransitionCombinatorViewControllerBase<T> RequestedTransitionAppear { get; private set; }

        public UITransitionCombinatorViewControllerBase<T> RequestedTransitionDisappear { get; private set; }

        public bool ShouldSetRequestedTransitionAppear { get; private set; }
        public bool ShouldSetRequestedTransitionDisappear { get; private set; }

        public override void OnValidate() { }

        protected virtual void Awake() {
            Subscribe();
        }

        protected abstract T GetThisInstance();

        protected void Subscribe() {
            Subject.OnWillAppear += Subject_OnWillAppear;
            Subject.OnDidAppear += Subject_OnDidAppear;
            Subject.OnWillDisappear += Subject_OnWillDisappear;
            Subject.OnDidDisappear += Subject_OnDidDisappear;
            Subject.OnUpdateTransition += Subject_OnUpdateTransition;
        }

        public void SetTransitionAppear(UITransitionCombinatorViewControllerBase<T> newTransition) {
            if (Subject.IsTransitioning) {
                RequestedTransitionAppear = newTransition;
                ShouldSetRequestedTransitionAppear = true;
            } else {
                transitionAppear = newTransition;
            }
        }

        public void SetTransitionDisappear(UITransitionCombinatorViewControllerBase<T> newTransition) {
            if (Subject.IsTransitioning) {
                RequestedTransitionDisappear = newTransition;
                ShouldSetRequestedTransitionDisappear = true;
            } else {
                transitionDisappear = newTransition;
            }
        }

        protected virtual void ResetParameters() { // This is for reset the parameters between transitions.
            if (transitionReset != null) {
                transitionReset.OnWillAppear(GetThisInstance());
                transitionReset.OnUpdateTransition(GetThisInstance(), 1f);
                transitionReset.OnDidAppear(GetThisInstance());
                transitionReset.OnCompleteTransition(GetThisInstance());
            }
        }

        protected virtual void Subject_OnWillAppear(UIViewController viewController) {
            ResetParameters();
            if (transitionAppear == null) {
                return;
            }

            transitionAppear.OnWillAppear(GetThisInstance());
        }

        protected virtual void Subject_OnDidAppear(UIViewController viewController) {
            if (transitionAppear == null) {
                return;
            }

            transitionAppear.OnDidAppear(GetThisInstance());

            if (ShouldSetRequestedTransitionAppear) {
                ShouldSetRequestedTransitionAppear = false;
                transitionAppear = RequestedTransitionAppear;
            }

            if (ShouldSetRequestedTransitionDisappear) {
                ShouldSetRequestedTransitionDisappear = false;
                transitionDisappear = RequestedTransitionDisappear;
            }
        }

        protected virtual void Subject_OnWillDisappear(UIViewController viewController) {
            ResetParameters();
            if (transitionDisappear == null) {
                return;
            }

            transitionDisappear.OnWillDisappear(GetThisInstance());
        }

        protected virtual void Subject_OnDidDisappear(UIViewController viewController) {
            if (transitionDisappear == null) {
                return;
            }

            transitionDisappear.OnDidDisappear(GetThisInstance());

            if (ShouldSetRequestedTransitionAppear) {
                ShouldSetRequestedTransitionAppear = false;
                transitionAppear = RequestedTransitionAppear;
            }

            if (ShouldSetRequestedTransitionDisappear) {
                ShouldSetRequestedTransitionDisappear = false;
                transitionDisappear = RequestedTransitionDisappear;
            }
        }

        private void Subject_OnUpdateTransition(UIViewController viewController, float time) {
            UITransitionCombinatorViewControllerBase<T> transition = CurrentTransition;

            if (viewController.Animated && transition != null) {
                transition.OnUpdateTransition(GetThisInstance(), time);
            }
        }
    }
}