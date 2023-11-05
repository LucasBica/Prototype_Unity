using System;
using System.Collections.Generic;

using LB.TweenSystem.Runtime;

using LB.UIKit.Runtime.Serializables;

using UnityEngine;
using UnityEngine.EventSystems;

namespace LB.UIKit.Runtime.Components {

    [RequireComponent(typeof(UIButton))]
    public class UIToggle : UIObserver<UIButton> {

        [Header("Settings")]
        [SerializeField] protected bool shouldSetValueOnClick = true;
        [SerializeField] protected bool isOn = true;

        [Header("Assets")]
        [SerializeField] protected UITransitionCombinatorToggle transition = default;

        [Header("References")]
        [SerializeField] protected UIToggleGroup toggleGroup = default;
        [SerializeField] protected RectTransform rectContent = default;

        protected uint tweenId;
        protected Tween tween;

        protected Dictionary<UITransitionView, object> keyValuesTransitionData = new Dictionary<UITransitionView, object>();

        public event Action<UIToggle> OnIsOn;
        public event Action<UIToggle> OnIsOff;
        public event Action<UIToggle, bool> OnIsOnValueChanged;
        public event Action<UIToggle, float> OnUpdateTransition;
        public event Action<UIToggle, UITransitionCombinatorToggle> OnCompleteTransition;

        public virtual bool ShouldSetValueOnClick {
            get => shouldSetValueOnClick;
            set => shouldSetValueOnClick = value;
        }

        public virtual bool IsOn {
            get => isOn;
            set {
                if (isOn == value) {
                    return;
                }

                bool wasOn = isOn;
                bool hasToggleGroup = toggleGroup != null && toggleGroup.gameObject.activeInHierarchy && toggleGroup.enabled;

                if (hasToggleGroup) {
                    isOn = value || (!toggleGroup.AnyToggleOnExcept(this) && !toggleGroup.AllowSwitchOff);
                } else {
                    isOn = value;
                }

                if (isOn != wasOn) {
                    StopTween();
                    isOn = value;

                    if (isOn) OnIsOn?.Invoke(this);
                    else OnIsOff?.Invoke(this);
                    OnIsOnValueChanged?.Invoke(this, isOn);

                    if (transition != null) {
                        if (isOn) transition.OnIsOn(this);
                        else transition.OnIsOff(this);
                        NewTween();
                    }

                    if (hasToggleGroup) {
                        toggleGroup.NotifyToggleState(this);
                    }
                }
            }
        }

        public RectTransform RectContent => rectContent;

        public States State => isOn ? States.IsOn : States.IsOff;

        public override void OnValidate() {
            UIObserver<UIToggle>[] observers = GetComponents<UIObserver<UIToggle>>();

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

        private void Awake() {
            Subject.OnClick += Subject_OnClick;
        }

        protected virtual void OnEnable() {
            if (transition == null) {
                return;
            }

            transition.OnEnableView(this);
        }

        protected virtual void OnDisable() {
            StopTween();
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

        private void Subject_OnClick(UIButton button, PointerEventData eventData) {
            if (!ShouldSetValueOnClick) {
                return;
            }

            IsOn = !IsOn;
        }

        public enum States {
            IsOn,
            IsOff,
        }
    }
}