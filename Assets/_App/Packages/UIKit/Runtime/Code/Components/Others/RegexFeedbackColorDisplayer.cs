using UnityEngine;
using UnityEngine.UI;

namespace LB.UIKit.Runtime.Components {

    public class RegexFeedbackColorDisplayer : UIObserver<RegexFeedbackDisplayer> {

        [Header("Settings")]
        [SerializeField] private Color colorDefault = default;
        [SerializeField] private Color colorMatch = default;
        [SerializeField] private Color colorNoMatch = default;

        [Header("References")]
        [SerializeField] private Graphic[] graphics = default;

        public override void OnValidate() {

        }

        private void Awake() {
            Subject.OnMatchChanged += Subject_OnMatchChanged;
        }

        private void Subject_OnMatchChanged(RegexFeedbackDisplayer regexFeedbackDisplayer, bool? isMatch) {
            for (int i = 0; i < graphics.Length; i++) {
                if (isMatch == null) {
                    graphics[i].color = colorDefault;
                    continue;
                }

                graphics[i].color = isMatch.Value ? colorMatch : colorNoMatch;
            }
        }
    }
}