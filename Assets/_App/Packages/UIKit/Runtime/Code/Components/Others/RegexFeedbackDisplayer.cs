using System;
using System.Text.RegularExpressions;

using TMPro;

using UnityEngine;

namespace LB.UIKit.Runtime.Components {

    public class RegexFeedbackDisplayer : UIView {

        [Header("Settings")]
        [SerializeField] private string regularExpression = default;

        [Header("References")]
        [SerializeField] private TMP_InputField inputText = default;

        private Regex regex;

        public event Action<RegexFeedbackDisplayer, bool?> OnMatchChanged;

        private void OnValidate() {
            regex = new Regex(regularExpression, RegexOptions.None);
        }

        private void Awake() {
            regex = new Regex(regularExpression, RegexOptions.None);
            inputText.onValueChanged.AddListener(InputText_onValueChanged);
        }

        private void InputText_onValueChanged(string text) {
            OnMatchChanged?.Invoke(this, string.IsNullOrEmpty(text) ? null : regex.IsMatch(text));
        }
    }
}