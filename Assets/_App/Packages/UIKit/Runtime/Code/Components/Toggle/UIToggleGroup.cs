using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;

namespace LB.UIKit.Runtime.Components {

    [DisallowMultipleComponent]
    public class UIToggleGroup : UIBehaviour {

        [Header("Settings")]
        [SerializeField] private int maxTogglesOn = 1;
        [SerializeField] private bool allowSwitchOff = false;

        protected readonly List<UIToggle> toggleListeners = new List<UIToggle>(4);
        protected Queue<UIToggle> togglesOn = new Queue<UIToggle>(4);

        public int MaxTogglesOn {
            get => maxTogglesOn;
            set => maxTogglesOn = value < 1 ? 1 : value;
        }

        public bool AllowSwitchOff {
            get => allowSwitchOff;
            set => allowSwitchOff = value;
        }

        public int ToggleListenersCount => toggleListeners.Count;

        protected override void Start() {
            if (maxTogglesOn < 1) {
                maxTogglesOn = 1;
            }

            EnsureValidState();
            base.Start();
        }

        protected override void OnEnable() {
            EnsureValidState();
            base.OnEnable();
        }

        public void UnregisterToggle(UIToggle toggle) {
            if (toggleListeners.Contains(toggle)) {
                toggleListeners.Remove(toggle);
            }
        }

        public void RegisterToggle(UIToggle toggle) {
            if (!toggleListeners.Contains(toggle)) {
                toggleListeners.Add(toggle);
            }
        }

        public void NotifyToggleState(UIToggle toggle) {
            ValidateToggleIsInGroup(toggle);
            ValidateToggleShouldBeInStack(toggle);
            EnsureValidState();
        }

        private void ValidateToggleIsInGroup(UIToggle toggle) {
            if (toggle == null || !toggleListeners.Contains(toggle)) {
                throw new ArgumentException($"Toggle {toggle} is not part of ToggleGroup {this}");
            }
        }

        private void ValidateToggleShouldBeInStack(UIToggle toggle) {
            if (toggle.IsOn && !togglesOn.Contains(toggle)) {
                togglesOn.Enqueue(toggle);
            } else if (!toggle.IsOn && togglesOn.Contains(toggle)) {
                UIToggle[] stackArray = togglesOn.ToArray();
                UIToggle[] newStackArray = new UIToggle[stackArray.Length - 1];
                int index = 0;
                for (int i = 0; i < stackArray.Length; i++) {
                    if (stackArray[i] != toggle) {
                        newStackArray[index] = stackArray[i];
                        index++;
                    }
                }
                togglesOn = new Queue<UIToggle>(newStackArray);
            }
        }

        public void EnsureValidState() {
            if (!AllowSwitchOff && !AnyToggleOn() && toggleListeners.Count != 0) {
                toggleListeners[0].IsOn = true;
            }

            if (togglesOn.Count > maxTogglesOn) {
                int amountToRemove = togglesOn.Count - maxTogglesOn;
                for (int i = 0; i < amountToRemove; i++) {
                    togglesOn.Dequeue().IsOn = false;
                }
            }
        }

        public bool AnyToggleOn() {
            return togglesOn.Count > 0;
        }

        public bool AnyToggleOnExcept(UIToggle toggle) {
            if (togglesOn.Count == 0) {
                return false;
            }

            if (togglesOn.Count > 1) {
                return true;
            }

            return togglesOn.Peek() != toggle;
        }

        public void SetAllTogglesOff() {
            bool allowSwitchOff = this.allowSwitchOff;
            this.allowSwitchOff = true;

            for (int i = 0; i < togglesOn.Count; i++) {
                togglesOn.Dequeue().IsOn = false;
            }

            this.allowSwitchOff = allowSwitchOff;
        }
    }
}