using TMPro;
using UnityEngine;

namespace LB.UIKit.Runtime.Components {

    public class SetterPasswordChar : UIView {

        [Header("Settings")]
        [SerializeField] protected char passwordChar = '•';

        [Header("References")]
        [SerializeField] protected TMP_InputField inputfield = default;

        protected virtual void Awake() {
            inputfield.asteriskChar = passwordChar;
        }
    }
}