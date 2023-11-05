using System;

using LB.TweenSystem.Runtime;
using LB.UIKit.Runtime.Extensions;
using LB.UIKit.Runtime.Serializables;

using UnityEngine;

namespace LB.UIKit.Runtime.Components {

    public class UIViewController : UIView {

        [Header("Settings")]
        [SerializeField] private string displayName = default;
        [SerializeField] private States state = States.Disappeared;

        [Header("Assets")]
        [SerializeField] private UITransitionCombinatorViewController transitionReset = default;
        [SerializeField] private UITransitionCombinatorViewController transitionAppear = default;
        [SerializeField] private UITransitionCombinatorViewController transitionDisappear = default;

        [Header("References")]
        [SerializeField] private CanvasGroup canvasGroup = default;
        [SerializeField] private RectTransform rectContent = default;

        private uint tweenId;
        private Tween tween;

        public event Action<UIViewController> OnWillAppear;
        public event Action<UIViewController> OnDidAppear;
        public event Action<UIViewController> OnWillDisappear;
        public event Action<UIViewController> OnDidDisappear;
        public event Action<UIViewController, float> OnUpdateTransition;

        public string DisplayName => displayName;

        public States State => state;

        private Canvas canvas;
        public Canvas Canvas {
            get {
                if (canvas == null) {
                    canvas = RectT.GetCanvasRoot();
                }
                return canvas;
            }
        }

        public CanvasGroup CanvasGroup {
            get {
                if (canvasGroup == null) {
                    canvasGroup = gameObject.AddComponent<CanvasGroup>();
                }
                return canvasGroup;
            }
        }

        public RectTransform RectContent => rectContent;

        public bool IsTransitioning => state == States.Appearing || state == States.Disappearing;

        public UITransitionCombinatorViewController CurrentTransition => state == States.Appearing ? transitionAppear : state == States.Disappearing ? transitionDisappear : null;

        public UITransitionCombinatorViewController RequestedTransitionAppear { get; private set; }

        public UITransitionCombinatorViewController RequestedTransitionDisappear { get; private set; }

        public bool ShouldSetRequestedTransitionAppear { get; private set; }
        public bool ShouldSetRequestedTransitionDisappear { get; private set; }

        public bool Animated { get; private set; }

        protected virtual void OnValidate() {
            displayName = string.IsNullOrEmpty(displayName) ? string.Empty : displayName.ToLower().Replace(' ', '_');
        }

        public void SetTransitionAppear(UITransitionCombinatorViewController newTransition) {
            if (IsTransitioning) {
                RequestedTransitionAppear = newTransition;
                ShouldSetRequestedTransitionAppear = true;
            } else {
                transitionAppear = newTransition;
            }
        }

        public void SetTransitionDisappear(UITransitionCombinatorViewController newTransition) {
            if (IsTransitioning) {
                RequestedTransitionDisappear = newTransition;
                ShouldSetRequestedTransitionDisappear = true;
            } else {
                transitionDisappear = newTransition;
            }
        }

        public void SetState(bool active, bool animated) {
            if (active) {
                Appear(animated);
            } else {
                Disappear(animated);
            }
        }

        [ContextMenu(nameof(Appear))]
        private void Appear() => Appear(true);
        public void Appear(bool animated) {
            Animated = animated;
            tween = Tween.StopIfHasSameId(tweenId, tween);
            state = States.Appearing;
            ResetParameters();

            if (transitionAppear != null) {
                transitionAppear.OnWillAppear(this);
            }

            OnWillAppear?.Invoke(this);

            StartTransition();
        }

        [ContextMenu(nameof(Disappear))]
        private void Disappear() => Disappear(true);
        public void Disappear(bool animated) {
            Animated = animated;
            tween = Tween.StopIfHasSameId(tweenId, tween);
            state = States.Disappearing;
            ResetParameters();

            if (transitionDisappear != null) {
                transitionDisappear.OnWillDisappear(this);
            }

            OnWillDisappear?.Invoke(this);

            StartTransition();
        }

        private void StartTransition() {
            if (!IsTransitioning) {
                Debug.LogError("Should be setted a state of transition (appearing or disappearing)");
                return;
            }

            UITransitionCombinatorViewController transition = CurrentTransition;

            if (Animated && transition != null && transition.TransitionTime > 0f) {
                tween = Tween.New(transition.TransitionTime).SetUpdateAction(OnUpdateTransitionHandler).SetCompleteAction(OnCompleteTransitionHandler);
                tweenId = tween.Id;
            } else {
                OnUpdateTransitionHandler(Tween.INSTANT);
                OnCompleteTransitionHandler(Tween.INSTANT);
            }
        }

        private void ResetParameters() { // This is for reset the parameters between transitions.
            if (transitionReset != null) {
                transitionReset.OnWillAppear(this);
                transitionReset.OnUpdateTransition(this, 1f);
                transitionReset.OnDidAppear(this);
                transitionReset.OnCompleteTransition(this);
            }
        }

        private void Error() {
            Debug.LogError($"[{nameof(UIViewController)}] Something was wrong");
        }

        private void OnUpdateTransitionHandler(Tween tween) {
            float time = 0f;

            if (state == States.Appearing) {
                time = tween.Time;
            } else if (state == States.Disappearing) {
                time = 1f - tween.Time;
            } else {
                Error();
                return;
            }

            UITransitionCombinatorViewController transition = CurrentTransition;

            if (Animated && transition != null) {
                transition.OnUpdateTransition(this, time);
                OnUpdateTransition?.Invoke(this, time);
            }
        }

        private void OnCompleteTransitionHandler(Tween tween) {
            UITransitionCombinatorViewController transition = CurrentTransition;

            if (state == States.Appearing) {
                state = States.Appeared;

                if (transition != null) {
                    transition.OnDidAppear(this);
                }

                OnDidAppear?.Invoke(this);
            } else if (state == States.Disappearing) {
                state = States.Disappeared;

                if (transition != null) {
                    transition.OnDidDisappear(this);
                }

                OnDidDisappear?.Invoke(this);
            } else {
                Error();
                return;
            }

            if (ShouldSetRequestedTransitionAppear) {
                ShouldSetRequestedTransitionAppear = false;
                transitionAppear = RequestedTransitionAppear;
            }

            if (ShouldSetRequestedTransitionDisappear) {
                ShouldSetRequestedTransitionDisappear = false;
                transitionDisappear = RequestedTransitionDisappear;
            }
        }

        public enum States {
            Disappeared,
            Appearing,
            Appeared,
            Disappearing,
        }
    }
}