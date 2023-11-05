using System;
using System.Collections.Generic;

using LB.TweenSystem.Runtime;

using LB.UIKit.Runtime.Serializables;

using UnityEngine;
using UnityEngine.EventSystems;

namespace LB.UIKit.Runtime.Components {

    public class UIButton : UIView, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IPointerClickHandler, ISelectHandler, IDeselectHandler {

        [Header("Settings")]
        [SerializeField] protected bool interactive = true;

        [Header("Assets")]
        [SerializeField] protected UITransitionCombinatorButton transition = default;

        [Header("References")]
        [SerializeField] protected RectTransform rectContent = default;

        protected States state = States.Normal;
        protected InteractableObjectEvents lastReveivedEvent = InteractableObjectEvents.None;
        protected uint tweenId;
        protected Tween tween;

        protected bool canvasGroupAllowInteraction;
        private readonly List<CanvasGroup> canvasGroups = new();

        protected Dictionary<UITransitionView, object> keyValuesTransitionData = new();

        public event Action<UIButton, bool> OnInteractiveValueChanged;
        public event Action<UIButton, BaseEventData> OnSelectHandler;
        public event Action<UIButton, BaseEventData> OnDeselectHandler;
        public event Action<UIButton, PointerEventData> OnEnter;
        public event Action<UIButton, PointerEventData> OnDown;
        public event Action<UIButton, PointerEventData> OnUp;
        public event Action<UIButton, PointerEventData> OnExit;
        public event Action<UIButton, PointerEventData> OnClick;
        public event Action<UIButton, float> OnUpdateTransition;
        public event Action<UIButton, UITransitionCombinatorButton> OnCompleteTransition;

        public States State => state;

        public InteractableObjectEvents LastReveivedEvent => lastReveivedEvent;

        public virtual bool IsInteractive {
            get => interactive;
            set {
                if (interactive == value) {
                    return;
                }

                StopTween();
                interactive = value;
                lastReveivedEvent = InteractableObjectEvents.InteractiveValueChanged;
                state = GetCurrentState();

                OnInteractiveValueChanged?.Invoke(this, interactive);

                if (transition != null) {
                    transition.OnInteractiveValueChanged(this, interactive);
                    NewTween();
                }
            }
        }

        public RectTransform RectContent => rectContent;

        protected virtual void OnValidate() {
            UIObserver<UIButton>[] observers = GetComponents<UIObserver<UIButton>>();

            for (int i = 0; i < observers.Length; i++) {
                observers[i].OnValidate();
            }

            if (transition == null) {
                return;
            }

            if (Application.isPlaying) {
                StopTween();
            }

            transition.OnValidateView(this);
        }

        protected virtual void Awake() { }

        protected virtual void OnEnable() {
            if (transition == null) {
                return;
            }

            transition.OnEnableView(this);
        }

        protected virtual void OnDisable() {
            StopTween();
        }

        protected virtual void OnCanvasGroupChanged() {
            var parentGroupAllowsInteraction = ParentGroupAllowsInteraction();

            if (parentGroupAllowsInteraction != canvasGroupAllowInteraction) {
                canvasGroupAllowInteraction = parentGroupAllowsInteraction;
            }
        }

        protected virtual bool ParentGroupAllowsInteraction() {
            Transform t = transform;
            while (t != null) {
                t.GetComponents(canvasGroups);
                for (var i = 0; i < canvasGroups.Count; i++) {
                    if (canvasGroups[i].enabled && !canvasGroups[i].interactable)
                        return false;

                    if (canvasGroups[i].ignoreParentGroups)
                        return true;
                }

                t = t.parent;
            }

            return true;
        }

        public virtual object GetTransitionData(UITransitionView transitionButton) {
            if (keyValuesTransitionData.TryGetValue(transitionButton, out object value)) {
                return value;
            }
            return null;
        }

        public virtual void SetTransitionData(UITransitionView transitionButton, object data) {
            if (keyValuesTransitionData.ContainsKey(transitionButton)) {
                keyValuesTransitionData[transitionButton] = data;
            } else {
                keyValuesTransitionData.Add(transitionButton, data);
            }
        }

        protected virtual States GetCurrentState() {
            if (!interactive) {
                return States.Disabled;
            }

            switch (lastReveivedEvent) {
                case InteractableObjectEvents.None:
                    return States.Normal;
                case InteractableObjectEvents.InteractiveValueChanged:
                    return States.Normal;
                case InteractableObjectEvents.Select:
                    return States.Selected;
                case InteractableObjectEvents.Deselected:
                    return States.Selected;
                case InteractableObjectEvents.Enter:
                    return States.Highlighted;
                case InteractableObjectEvents.Down:
                    return States.Pressed;
                case InteractableObjectEvents.Up:
                    return States.Normal;
                case InteractableObjectEvents.Exit:
                    return States.Normal;
                case InteractableObjectEvents.Click:
                    return States.Normal;
                default:
                    Debug.LogWarning(new Exception($"[{nameof(UIButton)}] Invalid {nameof(InteractableObjectEvents)} type: {lastReveivedEvent}"));
                    return States.Normal;
            }
        }

        protected virtual void NewTween() {
            if (!Application.isPlaying || transition == null || transition.TransitionTime <= 0 || !isActiveAndEnabled) {
                return;
            }

            tween = Tween.New(transition.TransitionTime).SetUpdateAction(OnUpdateTransitionHandler).SetCompleteAction(OnCompleteTransitionHandler);
            tweenId = tween.Id;
        }

        protected virtual void StopTween() {
            tween = Tween.StopIfHasSameId(tweenId, tween, true, true);
            tweenId = 0;
        }

        public virtual void OnSelect(BaseEventData eventData) {
            if (!interactive || !canvasGroupAllowInteraction) {
                return;
            }

            StopTween();
            lastReveivedEvent = InteractableObjectEvents.Select;
            state = GetCurrentState();

            OnSelectHandler?.Invoke(this, eventData);

            if (transition != null) {
                transition.OnSelect(this, eventData);
                NewTween();
            }
        }

        public virtual void OnDeselect(BaseEventData eventData) {
            StopTween();
            lastReveivedEvent = InteractableObjectEvents.Deselected;
            state = GetCurrentState();

            OnDeselectHandler?.Invoke(this, eventData);

            if (transition != null) {
                transition.OnDeselect(this, eventData);
                NewTween();
            }
        }

        public virtual void OnPointerEnter(PointerEventData eventData) {
            if (!interactive || !eventData.eligibleForClick || !canvasGroupAllowInteraction) { // We use "eventData.eligibleForClick" because "OnPointerEnter" could be called after "OnPointerExit" without exit from the rect of the button after click it.
                return;
            }

            StopTween();
            lastReveivedEvent = InteractableObjectEvents.Enter;
            state = GetCurrentState();

            OnEnter?.Invoke(this, eventData);

            if (transition != null) {
                transition.OnPointerEnter(this, eventData);
                NewTween();
            }
        }

        public virtual void OnPointerDown(PointerEventData eventData) {
            if (!interactive || !canvasGroupAllowInteraction) {
                return;
            }

            StopTween();
            lastReveivedEvent = InteractableObjectEvents.Down;
            state = GetCurrentState();

            OnDown?.Invoke(this, eventData);

            if (transition != null) {
                transition.OnPointerDown(this, eventData);
                NewTween();
            }
        }

        public virtual void OnPointerUp(PointerEventData eventData) {
            if (!interactive) {
                return;
            }

            StopTween();
            lastReveivedEvent = InteractableObjectEvents.Up;
            state = GetCurrentState();

            OnUp?.Invoke(this, eventData);

            if (transition != null) {
                transition.OnPointerUp(this, eventData);
                NewTween();
            }
        }

        public virtual void OnPointerExit(PointerEventData eventData) {
            if (!interactive) {
                return;
            }

            StopTween();
            lastReveivedEvent = InteractableObjectEvents.Exit;
            state = GetCurrentState();

            OnExit?.Invoke(this, eventData);

            if (transition != null) {
                transition.OnPointerExit(this, eventData);
                NewTween();
            }
        }

        public virtual void OnPointerClick(PointerEventData eventData) {
            if (!interactive || !canvasGroupAllowInteraction) {
                return;
            }

            StopTween();
            lastReveivedEvent = InteractableObjectEvents.Click;
            state = GetCurrentState();

            OnClick?.Invoke(this, eventData);

            if (transition != null) {
                transition.OnPointerClick(this, eventData);
                NewTween();
            }
        }

        protected virtual void OnUpdateTransitionHandler(Tween tween) {
            if (transition == null) {
                return;
            }

            float time = tween.Time;

            transition.OnUpdateTransition(this, time);
            OnUpdateTransition?.Invoke(this, time);
        }

        protected virtual void OnCompleteTransitionHandler(Tween tween) {
            if (transition == null) {
                return;
            }

            transition.OnCompleteTransition(this);
            OnCompleteTransition?.Invoke(this, transition);
        }

        public enum States {
            Normal,
            Selected,
            Highlighted,
            Pressed,
            Disabled,
        }

        public enum InteractableObjectEvents {
            None,
            InteractiveValueChanged,
            Select,
            Deselected,
            Enter,
            Down,
            Up,
            Exit,
            Click,
        }
    }
}